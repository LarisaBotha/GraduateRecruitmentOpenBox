using GraduateRecruitment.ConsoleApp.Data;
using GraduateRecruitment.ConsoleApp.Classes;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System;

namespace GraduateRecruitment.UnitTests.Classes
{
    [TestFixture]
    internal class InventoryLibraryTests
    {
        
        [Test]
        public void testAll(){

            OpenBarRepository repo = new OpenBarRepository();
            var library = new InventoryLibrary(repo);

            using(new AssertionScope()){
                foreach(var item in repo.AllInventory){  
                    
                    library.getInventoryNameById(item.Id).Should().Be(item.Name);
                    library.getInventoryIdByName(item.Name).Should().Be(item.Id);
                    library.getInventoryPriceById(item.Id).Should().Be(item.Price);
                    library.getInventoryPriceByName(item.Name).Should().Be(item.Price);

                    /*
                    Assert.AreEqual(item.Name,library.getInventoryNameById(item.Id));
                    Assert.AreEqual(item.Id,library.getInventoryIdByName(item.Name));
                    Assert.AreEqual(item.Price,library.getInventoryPriceById(item.Id));
                    Assert.AreEqual(item.Price,library.getInventoryPriceByName(item.Name));
                    */
                }
            }
        }
    }

    
}