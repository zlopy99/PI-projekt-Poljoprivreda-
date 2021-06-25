using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PI_projekt.Models
{
    public partial class PI07Context : DbContext
    {

        public PI07Context(DbContextOptions<PI07Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Alat> Alats { get; set; }
        public virtual DbSet<Biljka> Biljkas { get; set; }
        public virtual DbSet<BiljkaUpotreba> BiljkaUpotrebas { get; set; }
        public virtual DbSet<BiljkeKorisnik> BiljkeKorisniks { get; set; }
        public virtual DbSet<DetaljiNarudžbe> DetaljiNarudžbes { get; set; }
        public virtual DbSet<FazaRazvoja> FazaRazvojas { get; set; }
        public virtual DbSet<FazaRazvojaBiljke> FazaRazvojaBiljkes { get; set; }
        public virtual DbSet<Korisnik> Korisniks { get; set; }
        public virtual DbSet<Kupac> Kupacs { get; set; }
        public virtual DbSet<Lokacija> Lokacijas { get; set; }
        public virtual DbSet<Mikrolokacija> Mikrolokacijas { get; set; }
        public virtual DbSet<NarodnaImena> NarodnaImenas { get; set; }
        public virtual DbSet<Narudžba> Narudžbas { get; set; }
        public virtual DbSet<OstvareneNarudzbe> OstvareneNarudzbes { get; set; }
        public virtual DbSet<PlaniraniTroškovi> PlaniraniTroškovis { get; set; }
        public virtual DbSet<Porodica> Porodicas { get; set; }
        public virtual DbSet<Poslovi> Poslovis { get; set; }
        public virtual DbSet<PosloviBiljka> PosloviBiljkas { get; set; }
        public virtual DbSet<ProdajaBezNarudzbe> ProdajaBezNarudzbes { get; set; }
        public virtual DbSet<Razred> Razreds { get; set; }
        public virtual DbSet<Red> Reds { get; set; }
        public virtual DbSet<Rod> Rods { get; set; }
        public virtual DbSet<TipUzgoja> TipUzgojas { get; set; }
        public virtual DbSet<Troškovi> Troškovis { get; set; }
        public virtual DbSet<Upotreba> Upotrebas { get; set; }
        public virtual DbSet<Urod> Urods { get; set; }
        public virtual DbSet<VrstaTla> VrstaTlas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Alat>(entity =>
            {
                entity.ToTable("Alat");

                entity.Property(e => e.AlatId).HasColumnName("alat_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.KorisnikNavigation)
                    .WithMany(p => p.Alats)
                    .HasForeignKey(d => d.Korisnik)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Alat_Korisnik");
            });

            modelBuilder.Entity<Biljka>(entity =>
            {
                entity.ToTable("Biljka");

                entity.Property(e => e.BiljkaId).HasColumnName("biljka_id");

                entity.Property(e => e.Naziv).IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.Property(e => e.Slika).IsUnicode(false);

                entity.Property(e => e.Vrsta)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.PorodicaNavigation)
                    .WithMany(p => p.Biljkas)
                    .HasForeignKey(d => d.Porodica)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Biljka_Porodica");

                entity.HasOne(d => d.RazredNavigation)
                    .WithMany(p => p.Biljkas)
                    .HasForeignKey(d => d.Razred)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Biljka_Razred");

                entity.HasOne(d => d.RedNavigation)
                    .WithMany(p => p.Biljkas)
                    .HasForeignKey(d => d.Red)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Biljka_Red");

                entity.HasOne(d => d.RodNavigation)
                    .WithMany(p => p.Biljkas)
                    .HasForeignKey(d => d.Rod)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Biljka_Rod");
            });

            modelBuilder.Entity<BiljkaUpotreba>(entity =>
            {
                entity.ToTable("Biljka_upotreba");

                entity.Property(e => e.BiljkaUpotrebaId).HasColumnName("biljka_upotreba_id");

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.HasOne(d => d.BiljkaNavigation)
                    .WithMany(p => p.BiljkaUpotrebas)
                    .HasForeignKey(d => d.Biljka)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Biljka_upotreba_Biljka");

                entity.HasOne(d => d.UpotrebaNavigation)
                    .WithMany(p => p.BiljkaUpotrebas)
                    .HasForeignKey(d => d.Upotreba)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Biljka_upotreba_Upotreba");
            });

            modelBuilder.Entity<BiljkeKorisnik>(entity =>
            {
                entity.HasKey(e => e.BiljkaKorisnikId);

                entity.ToTable("Biljke_korisnik");

                entity.Property(e => e.BiljkaKorisnikId).HasColumnName("biljka_korisnik_id");

                entity.Property(e => e.CijenaPoKg)
                    .IsUnicode(false)
                    .HasColumnName("Cijena_po_kg");

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.HasOne(d => d.BiljkaNavigation)
                    .WithMany(p => p.BiljkeKorisniks)
                    .HasForeignKey(d => d.Biljka)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Biljke_korisnik_Biljka");

                entity.HasOne(d => d.KorisnikNavigation)
                    .WithMany(p => p.BiljkeKorisniks)
                    .HasForeignKey(d => d.Korisnik)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Biljke_korisnik_Korisnik");
            });

            

            modelBuilder.Entity<DetaljiNarudžbe>(entity =>
            {
                entity.HasKey(e => e.DetaljiNarudzbeId);

                entity.ToTable("Detalji_narudžbe");

                entity.Property(e => e.DetaljiNarudzbeId).HasColumnName("detalji_narudzbe_id");

                entity.Property(e => e.kolicina).HasColumnName("kolicina");

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.Property(e => e.UkupnaCijena)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Ukupna_cijena");

                entity.HasOne(d => d.BiljkaNavigation)
                    .WithMany(p => p.DetaljiNarudžbes)
                    .HasForeignKey(d => d.Biljka)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Detalji_narudžbe_Biljke_korisnik");

                entity.HasOne(d => d.NarudzbaNavigation)
                    .WithMany(p => p.DetaljiNarudžbes)
                    .HasForeignKey(d => d.Narudzba)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Detalji_narudžbe_Narudžba");
            });

            modelBuilder.Entity<FazaRazvoja>(entity =>
            {
                entity.ToTable("Faza_razvoja");

                entity.Property(e => e.FazaRazvojaId).HasColumnName("faza_razvoja_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FazaRazvojaBiljke>(entity =>
            {
                entity.ToTable("Faza_razvoja_biljke");

                entity.Property(e => e.FazaRazvojaBiljkeId).HasColumnName("faza_razvoja_biljke_id");

                entity.Property(e => e.FazaRazvoja).HasColumnName("Faza_razvoja");

                entity.Property(e => e.GodisnjeDobaOdvijanjaFaze)
                    .IsUnicode(false)
                    .HasColumnName("Godisnje_doba_odvijanja_faze");

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.Property(e => e.TrajanjeFaze)
                    .IsUnicode(false)
                    .HasColumnName("Trajanje_faze");

                entity.HasOne(d => d.BiljkaNavigation)
                    .WithMany(p => p.FazaRazvojaBiljkes)
                    .HasForeignKey(d => d.Biljka)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Faza_razvoja_biljke_Biljka");

                entity.HasOne(d => d.FazaRazvojaNavigation)
                    .WithMany(p => p.FazaRazvojaBiljkes)
                    .HasForeignKey(d => d.FazaRazvoja)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Faza_razvoja_biljke_Faza_razvoja");
            });

            modelBuilder.Entity<Korisnik>(entity =>
            {
                entity.ToTable("Korisnik");

                entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.ImePrezime)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Ime_prezime");

                entity.Property(e => e.Lozinka).IsUnicode(false);
            });

            modelBuilder.Entity<Kupac>(entity =>
            {
                entity.ToTable("Kupac");

                entity.Property(e => e.KupacId).HasColumnName("kupac_id");

                entity.Property(e => e.Adresa).IsUnicode(false);

                entity.Property(e => e.ImePrezime)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Ime_prezime");

                entity.Property(e => e.Kontkat).IsUnicode(false);
            });

            modelBuilder.Entity<Lokacija>(entity =>
            {
                entity.ToTable("Lokacija");

                entity.Property(e => e.LokacijaId).HasColumnName("lokacija_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.ObrađenaPovrsina)
                    .IsUnicode(false)
                    .HasColumnName("Obrađena_povrsina");

                entity.Property(e => e.UkupnaPovrsinaParcele)
                    .IsUnicode(false)
                    .HasColumnName("Ukupna_povrsina_parcele");

                entity.Property(e => e.VrstaTla).HasColumnName("Vrsta_tla");

                entity.Property(e => e.Korisnik).HasColumnName("Korisnik");

                entity.HasOne(d => d.VrstaTlaNavigation)
                    .WithMany(p => p.Lokacijas)
                    .HasForeignKey(d => d.VrstaTla)
                    .HasConstraintName("FK_Lokacija_Vrsta_tla");

                entity.HasOne(d => d.KorisnikNavigation)
                    .WithMany(p => p.Lokacijas)
                    .HasForeignKey(d => d.Korisnik)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Lokacija_Korisnik");
            });

            modelBuilder.Entity<Mikrolokacija>(entity =>
            {
                entity.ToTable("Mikrolokacija");

                entity.Property(e => e.MikrolokacijaId).HasColumnName("mikrolokacija_id");

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.Property(e => e.OčekivaniUrod)
                    .IsUnicode(false)
                    .HasColumnName("Očekivani_urod");

                entity.Property(e => e.Površina).IsUnicode(false);

                entity.Property(e => e.TipUzgoja).HasColumnName("Tip_uzgoja");

                entity.HasOne(d => d.BiljkaNavigation)
                    .WithMany(p => p.Mikrolokacijas)
                    .HasForeignKey(d => d.Biljka)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mikrolokacija_Biljke_korisnik");

                entity.HasOne(d => d.LokacijaNavigation)
                    .WithMany(p => p.Mikrolokacijas)
                    .HasForeignKey(d => d.Lokacija)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Mikrolokacija_Lokacija");

                entity.HasOne(d => d.TipUzgojaNavigation)
                    .WithMany(p => p.Mikrolokacijas)
                    .HasForeignKey(d => d.TipUzgoja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mikrolokacija_Tip_uzgoja");
            });

            modelBuilder.Entity<NarodnaImena>(entity =>
            {
                entity.HasKey(e => e.NarodnoImeId);

                entity.ToTable("Narodna_imena");

                entity.Property(e => e.NarodnoImeId).HasColumnName("narodno_ime_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.BiljkaNavigation)
                    .WithMany(p => p.NarodnaImenas)
                    .HasForeignKey(d => d.Biljka)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Narodna_imena_Biljka");
            });

            modelBuilder.Entity<Narudžba>(entity =>
            {
                entity.HasKey(e => e.NaruzdbaId);

                entity.ToTable("Narudžba");

                entity.Property(e => e.NaruzdbaId).HasColumnName("naruzdba_id");

                entity.Property(e => e.DatumIsporuke)
                    .HasColumnType("datetime")
                    .HasColumnName("Datum_isporuke");

                entity.Property(e => e.DatumNarudzbe)
                    .HasColumnType("datetime")
                    .HasColumnName("Datum_narudzbe");

                entity.HasOne(d => d.KupacNavigation)
                    .WithMany(p => p.Narudžbas)
                    .HasForeignKey(d => d.Kupac)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Narudžba_Kupac");

                entity.HasOne(d => d.KorisnikNavigation)
                    .WithMany(p => p.Narudžbas)
                    .HasForeignKey(d => d.Korisnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Narudžba_Korisnik");
            });

            modelBuilder.Entity<OstvareneNarudzbe>(entity =>
            {
                entity.ToTable("Ostvarene_narudzbe");

                entity.Property(e => e.OstvareneNarudzbeId).HasColumnName("ostvarene_narudzbe_id");

                entity.Property(e => e.Biljka).IsUnicode(false);


                entity.Property(e => e.Datum).HasColumnType("datetime");

                entity.Property(e => e.Kolicina).HasColumnName("Kolicina");

                entity.Property(e => e.Kupac).IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.Property(e => e.Zarada).IsUnicode(false);

                entity.HasOne(d => d.KorisnikNavigation)
                    .WithMany(p => p.OstvareneNarudzbes)
                    .HasForeignKey(d => d.Korisnik)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Ostvarene_narudzbe_Korisnik");
                entity.HasOne(d => d.KupacNavigation)
                    .WithMany(p => p.OstvareneNarudzbes)
                    .HasForeignKey(d => d.Kupac)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Ostvarene_narudzbe_Kupac");
                entity.HasOne(d => d.BiljkeKorisnikNavigation)
                    .WithMany(p => p.OstvareneNarudzbes)
                    .HasForeignKey(d => d.Biljka)
                    .HasConstraintName("FK_Ostvarene_narudzbe_Biljke_korisnik");
            });

            modelBuilder.Entity<PlaniraniTroškovi>(entity =>
            {
                entity.HasKey(e => e.PlaniraniTroskoviId);

                entity.ToTable("Planirani_troškovi");

                entity.Property(e => e.PlaniraniTroskoviId).HasColumnName("planirani_troskovi_id");

                entity.Property(e => e.Datum).HasColumnType("datetime");

                entity.Property(e => e.Iznos).IsUnicode(false);

                entity.Property(e => e.Namjena).IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.HasOne(d => d.KorisnikNavigation)
                    .WithMany(p => p.PlaniraniTroškovis)
                    .HasForeignKey(d => d.Korisnik)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Planirani_troškovi_Korisnik");
            });

            modelBuilder.Entity<Porodica>(entity =>
            {
                entity.ToTable("Porodica");

                entity.Property(e => e.PorodicaId).HasColumnName("porodica_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");
            });

            modelBuilder.Entity<Poslovi>(entity =>
            {
                entity.ToTable("Poslovi");

                entity.Property(e => e.PosloviId).HasColumnName("poslovi_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PosloviBiljka>(entity =>
            {
                entity.ToTable("Poslovi_biljka");

                entity.Property(e => e.PosloviBiljkaId).HasColumnName("poslovi_biljka_id");

                entity.Property(e => e.Opis).HasColumnType("text");

                /*entity.HasOne(d => d.AlatNavigation)
                    .WithMany(p => p.PosloviBiljkas)
                    .HasForeignKey(d => d.Alat)
                    .HasConstraintName("FK_Poslovi_biljka_Alat");*/

                entity.HasOne(d => d.BiljkaNavigation)
                    .WithMany(p => p.PosloviBiljkas)
                    .HasForeignKey(d => d.Biljka)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Poslovi_biljka_Biljke_korisnik");

                entity.HasOne(d => d.PosloviNavigation)
                    .WithMany(p => p.PosloviBiljkas)
                    .HasForeignKey(d => d.Poslovi)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Poslovi_biljka_Poslovi");
            });

            modelBuilder.Entity<ProdajaBezNarudzbe>(entity =>
            {
                entity.ToTable("Prodaja_bez_narudzbe");

                entity.Property(e => e.ProdajaBezNarudzbeId).HasColumnName("prodaja_bez_narudzbe_id");

                entity.Property(e => e.Kolicina).HasColumnName("Kolicina");

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.Property(e => e.UkupnaCijena)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("Ukupna_cijena");

                entity.HasOne(d => d.BiljkaNavigation)
                    .WithMany(p => p.ProdajaBezNarudzbes)
                    .HasForeignKey(d => d.Biljka)
                    .HasConstraintName("FK_Prodaja_bez_narudzbe_Biljke_korisnik");

                entity.HasOne(d => d.KupacNavigation)
                    .WithMany(p => p.ProdajaBezNarudzbes)
                    .HasForeignKey(d => d.Kupac)
                    .HasConstraintName("FK_Prodaja_bez_narudzbe_Kupac");
            });

            modelBuilder.Entity<Razred>(entity =>
            {
                entity.ToTable("Razred");

                entity.Property(e => e.RazredId).HasColumnName("razred_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");
            });

            modelBuilder.Entity<Red>(entity =>
            {
                entity.ToTable("Red");

                entity.Property(e => e.RedId).HasColumnName("red_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");
            });

            modelBuilder.Entity<Rod>(entity =>
            {
                entity.ToTable("Rod");

                entity.Property(e => e.RodId).HasColumnName("rod_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");
            });

            modelBuilder.Entity<TipUzgoja>(entity =>
            {
                entity.ToTable("Tip_uzgoja");

                entity.Property(e => e.TipUzgojaId).HasColumnName("tip_uzgoja_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");
            });

            modelBuilder.Entity<Troškovi>(entity =>
            {
                entity.HasKey(e => e.TroskoviId);

                entity.ToTable("Troškovi");

                entity.Property(e => e.TroskoviId).HasColumnName("troskovi_id");

                entity.Property(e => e.Datum).HasColumnType("datetime");

                entity.Property(e => e.Iznos).IsUnicode(false);

                entity.Property(e => e.Namjena).IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.HasOne(d => d.KorisnikNavigation)
                    .WithMany(p => p.Troškovis)
                    .HasForeignKey(d => d.Korisnik)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Troškovi_Korisnik");
            });

            modelBuilder.Entity<Upotreba>(entity =>
            {
                entity.ToTable("Upotreba");

                entity.Property(e => e.UpotrebaId).HasColumnName("upotreba_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Urod>(entity =>
            {
                entity.ToTable("Urod");

                entity.Property(e => e.UrodId).HasColumnName("urod_id");

                entity.Property(e => e.Opis).HasColumnType("text");

                entity.Property(e => e.Kolicina).HasColumnName("Kolicina");

                entity.HasOne(d => d.MikrolokacijaNavigation)
                    .WithMany(p => p.Urods)
                    .HasForeignKey(d => d.Mikrolokacija)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("FK_Urod_Mikrolokacija");
            });

            modelBuilder.Entity<VrstaTla>(entity =>
            {
                entity.ToTable("Vrsta_tla");

                entity.Property(e => e.VrstaTlaId).HasColumnName("vrsta_tla_id");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Opis).HasColumnType("text");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
