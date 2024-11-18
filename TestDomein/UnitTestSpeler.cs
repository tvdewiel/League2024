using LeagueBL.Exceptions;
using LeagueBL.Model;

namespace TestDomein
{
    public class UnitTestSpeler
    {
        private Speler jos;

        public UnitTestSpeler()
        {
            jos= new Speler(10, "Jos", 180, 80);
            jos.ZetRugnummer(55);
        }

        //[Fact]
        //public void ZetId_Valid()
        //{
        //    Speler s = new Speler(10, "Jos", 180, 80);
        //    Assert.Equal(10, s.Id);
        //    s.ZetId(1);
        //    Assert.Equal(1, s.Id);
        //}
        //[Fact]
        //public void ZetId_Invalid()
        //{
        //    Speler s = new Speler(10, "Jos", 180, 80);
        //    Assert.Throws<SpelerException>(() => s.ZetId(0));
        //    Assert.Equal(10, s.Id);
        //}
        //[Theory]
        //[InlineData(1)]
        //[InlineData(99)]
        //public void ZetRugnummer_Valid(int rugnr)
        //{
        //    jos.ZetRugnummer(rugnr);
        //    Assert.Equal(rugnr, jos.Rugnummer);
        //}
        //[Theory]
        //[InlineData(0)]
        //[InlineData(100)]
        //[InlineData(-1)]
        //public void ZetRugnummer_InValid(int rugnr)
        //{
        //    Assert.Throws<SpelerException>(() => jos.ZetRugnummer(rugnr));
        //    Assert.Equal(55, jos.Rugnummer);
        //}
        //[Theory]
        //[InlineData("lowie", "lowie")]
        //[InlineData("  lowie", "lowie")]
        //[InlineData("  lowie   ", "lowie")]
        //[InlineData("  lowie valk   ", "lowie valk")]
        //public void ZetNaam_Valid(string naamIn, string naamOut)
        //{
        //    jos.ZetNaam(naamIn);
        //    Assert.Equal(naamOut, jos.Naam);
        //}
        //[Theory]
        //[InlineData("")]
        //[InlineData("   ")]
        //[InlineData(null)]
        //[InlineData(" \n")]
        //[InlineData("  \r ")]
        //public void ZetNaam_Invalid(string naam)
        //{
        //    Assert.Throws<SpelerException>(() => jos.ZetNaam(naam));
        //    Assert.Equal("Jos", jos.Naam);
        //}
        //[Theory]
        //[InlineData(10, "jos", 150, 80)]
        //[InlineData(10, "jos", 150, null)]
        //[InlineData(10, "jos", null, null)]
        //public void Ctor_withID_Valid(int id, string naam, int? lengte, int? gewicht)
        //{
        //    Speler s = new Speler(id, naam, lengte, gewicht);
        //    Assert.Equal(id, s.Id);
        //    Assert.Equal(naam, s.Naam);
        //    Assert.Equal(lengte, s.Lengte);
        //    Assert.Equal(gewicht, s.Gewicht);
        //}
        //[Fact]
        //public void ZetTeam_Valid()
        //{
        //    Speler s = new Speler(10, "Jos", 180, 80);
        //    Team t = new Team(1, "Antwerpen");
        //    s.ZetTeam(t);
        //    Assert.Equal(t, s.Team);
        //    Assert.Contains(s, t.Spelers);
        //}
        //[Fact]
        //public void VerwijderTeam_Valid()
        //{
        //    Speler s = new Speler(10, "Jos", 180, 80);
        //    Team t = new Team(1, "Antwerpen");
        //    s.ZetTeam(t);
        //    s.VerwijderTeam();
        //    Assert.Null(s.Team);
        //    Assert.DoesNotContain(s, t.Spelers);
        //}
        [Fact]
        public void ZetTeamInvalid()
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Team t = new Team(1, "Antwerpen");
            Team t2 = new Team(1, "Antwerpen");
            s.ZetTeam(t);
            Assert.Throws<SpelerException>(() => s.ZetTeam(null));
            Assert.Throws<SpelerException>(() => s.ZetTeam(t2));
            Assert.Equal(t, s.Team);
            Assert.Contains(s,t.Spelers);
        }
    }
}