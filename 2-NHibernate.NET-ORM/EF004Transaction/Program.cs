using EF004CreateSession;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

public class Program
{
    private static void Main(string[] args)
    {
        using(var session = CreateSession())
        {
            using(var transaction = session.BeginTransaction())
            {
                var idTo = 12;
                var idFrom = 11;
                var amountToTransfer = 1000;
                var walletFrom = session.Get<Wallet>(idFrom);
                var walletTo = session.Get<Wallet>(idTo);
                walletFrom.Balance -= amountToTransfer;
                walletTo.Balance += amountToTransfer;
                session.Update(walletFrom);
                session.Update(walletTo);
                transaction.Commit();
            }
        }
    
    }

    private static ISession CreateSession()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var constr = config.GetSection("constr").Value;
        var mapper = new ModelMapper();
        //list all of type mapping from assembly
        mapper.AddMappings(typeof(Wallet).Assembly.ExportedTypes);
        //compile class mapping
        HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        //Console.WriteLine(domainMapping.AsString());
        var hbConfig = new Configuration();
        hbConfig.DataBaseIntegration(c => 
        {
            c.Driver<MicrosoftDataSqlClientDriver>();
            c.Dialect<MsSql2012Dialect>();
            c.ConnectionString = constr;
           // c.LogSqlInConsole = true;
            //c.LogFormattedSql=true;
        });
        hbConfig.AddMapping(domainMapping);
        var sessionFactory = hbConfig.BuildSessionFactory();
          var session = sessionFactory.OpenSession();
        return session;
    }  
}