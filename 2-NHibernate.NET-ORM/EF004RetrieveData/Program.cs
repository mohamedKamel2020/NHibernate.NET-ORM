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
                var wallets=session.Query<Wallet>(); 
                foreach(var wallet in wallets)
                {
                    Console.WriteLine(wallet);
                }  
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
        Console.WriteLine(domainMapping.AsString());
        var hbConfig = new Configuration();
        hbConfig.DataBaseIntegration(c => 
        {
            c.Driver<MicrosoftDataSqlClientDriver>();
            c.Dialect<MsSql2012Dialect>();
            c.ConnectionString = constr;
            c.LogSqlInConsole = true;
            c.LogFormattedSql=true;
        });
        hbConfig.AddMapping(domainMapping);
        var sessionFactory = hbConfig.BuildSessionFactory();
          var session = sessionFactory.OpenSession();
        return session;
    }  
}