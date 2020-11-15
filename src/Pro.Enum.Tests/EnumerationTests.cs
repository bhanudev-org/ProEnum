using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace Pro.Enum.Tests
{
    public class EnumerationTests
    {
        [Fact]
        public void Enumeration_GetAll()
        {
            var list = new List<BloodGroup>
            {
                BloodGroup.ABPositive,
                BloodGroup.ABNegative,
                
                BloodGroup.APositive,
                BloodGroup.ANegative,

                BloodGroup.BPositive,
                BloodGroup.BNegative,

                BloodGroup.OPositive,
                BloodGroup.ONegative
            };

            Enumeration.GetAll<BloodGroup>().ShouldAllBe(p=>list.Contains(p));
            list.ShouldAllBe(p=>Enumeration.GetAll<BloodGroup>().Contains(p));
        }

        [Fact]
        public void Enumeration_CompareTo()
        {
            var negative = BloodGroup.ABNegative;
            var positive = BloodGroup.ABPositive;

            negative.CompareTo(positive).ShouldBe(negative.Value.CompareTo(positive.Value));
        }

        [Fact]
        public void Enumeration_CheckProperties()
        {
            BloodGroup.ABNegative.ShouldNotBeNull();
            BloodGroup.ABNegative.Name.ShouldNotBeNull();
            BloodGroup.ABNegative.Value.ShouldNotBe(0);

            BloodGroup.ABNegative.Name.ShouldBe("AB -ve");
            BloodGroup.ABNegative.Value.ShouldBe(8);

            BloodGroup.BPositive.ToString().ShouldBe("B +ve");

            var groupBPositive = BloodGroup.BPositive;
            BloodGroup.BPositive.Equals(groupBPositive).ShouldBeTrue();
            BloodGroup.BNegative.Equals(groupBPositive).ShouldBeFalse();

            (groupBPositive == BloodGroup.BPositive).ShouldBeTrue();
            (groupBPositive != BloodGroup.BNegative).ShouldBeTrue();
            (groupBPositive == BloodGroup.BNegative).ShouldBeFalse();
        }
    }
}