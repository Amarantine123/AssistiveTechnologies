using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace NFine.Data
{
    /// <summary>
    /// DB  context
    /// </summary>
    public class NFineDbContext : DbContext
    {
        public NFineDbContext()
            : base("NFineDbContext")//connectionStringName
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //assambly map, get NFine.Mapping.DLL
            string assembleFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("NFine.Data.DLL", "NFine.Mapping.DLL").Replace("file:///", "");
            Assembly asm = Assembly.LoadFile(assembleFileName);
            var typesToRegister = asm.GetTypes()//return An array that contains all the types that are defined in this assembly.
            .Where(type => !String.IsNullOrEmpty(type.Namespace))//type.Namespace is not empty
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
