using OffregLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryHive.Writer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (OffregHive hive = OffregHive.Create())
            {
                using (var registryEntry = hive.Root.CreateSubKey("REGISTRY"))
                {
                    using (var machineEntry = registryEntry.CreateSubKey("MACHINE")) 
                    {
                        using (OffregKey key = machineEntry.CreateSubKey("SOFTWARE"))
                        {
                            using (var subKey = key.CreateSubKey("Contoso")) 
                            {
                                using (var finalKey = subKey.CreateSubKey("ContosoExpenses")) 
                                {
                                    // Set a value to a string
                                    finalKey.SetValue("FirstRun", "True");
                                }
                            }
                        }
                    }
                }

                // Delete the file if it exists - Offreg requires files to not exist.
                if (File.Exists("Registry.dat"))
                    File.Delete("Registry.dat");

                // Save it to disk - version 5.1 is Windows XP. This is a form of compatibility option.
                // Read more here: http://msdn.microsoft.com/en-us/library/ee210773.aspx
                hive.SaveHive("Registry.dat", 5, 1);
            }
        }
    }
}
