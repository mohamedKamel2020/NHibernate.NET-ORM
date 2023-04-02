using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace EF004CreateSession
{
    public class WalletMapping:ClassMapping<Wallet>   
    {
        public WalletMapping()
        {
            Id(x => x.Id, c =>
            {
                c.Generator(Generators.Identity);
                c.Type(NHibernateUtil.Int32);
                c.Column("Id");
                c.UnsavedValue(0);
            });
            Property(x => x.Holder, c =>
            {
                c.Length(50);
                c.Type(NHibernateUtil.AnsiString);
                c.NotNullable(true);
            });
            Property(x => x.Balance, c =>
            {
                c.Type(NHibernateUtil.Decimal);
                c.NotNullable(true);
            });
            Table("Wallets");
            
        }

    }
}
