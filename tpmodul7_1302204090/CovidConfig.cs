using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace tpmodul7_1302204090
{
	internal class CovidConfig
	{

		private static string GetCovidFilePath => Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "covid_config.json");

		public enum Satuan
		{
			Celcius = 1,
			Fahrenheit
		}

		public Satuan SatuanSuhu { get; private set; }
		public int BatasHariDemam { get; private set; }
		public string PesanDitolak { get; private set; }
		public string PesanDiterima { get; private set; }

		public CovidConfig()
		{
			var file = File.OpenText(GetCovidFilePath);
			Dictionary<string, JsonElement>? json = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(file.ReadToEnd());

			// Set property config
			UbahSatuan(json["satuan_suhu"].GetString());
			BatasHariDemam = json["batas_hari_demam"].GetInt32();
			PesanDitolak = json["pesan_ditolak"].GetString();
			PesanDiterima = json["pesan_diterima"].GetString();

		}

		// Meminta user untuk memasukan suhu dan hari demam
		// Lalu panggil method CekSuhu
		public void MasukGedung()
		{
			Console.WriteLine($"\nBerapa suhu badan anda saat ini ? Dalam nilai {SatuanSuhu}");
			string? inputSuhu = Console.ReadLine();
			float suhu = -1;
			if (inputSuhu != null)
			{
				suhu = float.Parse(inputSuhu);
			}

			Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam ?");
			string? inputHari = Console.ReadLine();
			int hariDemam = -1;
			if (inputHari != null)
			{
				hariDemam = int.Parse(inputHari);
			}

			CekSuhu(suhu, hariDemam);
		}

		// Cek jika suhu diantara kententuan satuan & jika hari demam < property BatasHariDemam
		// Mengeluarkan output property PesanDitolak jika pengecekan adalah false
		// Mengeluarkan output property PesanDiterima jika pengecekan adalah true
		public void CekSuhu(float suhu, int hariDemam)
		{
			bool suhuPass;
			bool hariPass = hariDemam < BatasHariDemam;

			switch (SatuanSuhu)
			{
				case Satuan.Celcius:
					suhuPass = suhu >= 36.5 && suhu <= 37.5;
					break;
				case Satuan.Fahrenheit:
					suhuPass = suhu >= 97.7 && suhu <= 99.5;
					break;
				default:
					suhuPass = false;
					break;
			}

			if (suhuPass && hariPass)
			{
				Console.WriteLine(PesanDiterima);
			}
			else
			{
				Console.WriteLine(PesanDitolak);
			}
		}

		// Ubah satuan dengan string
		public Satuan UbahSatuan(string satuanSuhu)
		{
			Satuan satuan;
			bool success = Enum.TryParse(satuanSuhu, true, out satuan);
			if (!success)
			{
				satuan = Satuan.Celcius;
			}

			return UbahSatuan(satuan);
		}

		// Ubah satuan dengan enum
		public Satuan UbahSatuan(Satuan satuan)
		{
			SatuanSuhu = satuan;
			return satuan;
		}

		
	}
}
