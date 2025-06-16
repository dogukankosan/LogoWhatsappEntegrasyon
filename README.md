
![indir (2)](https://github.com/user-attachments/assets/496e24a2-2e9f-4c53-be6d-cf2d44afbf0d)

# Logo Whatsapp Entegrasyon

📲 **Logo ERP'den veri çekip, WhatsApp (ve opsiyonel e‑posta) üzerinden anlık bildirim gönderen servis**

Bu proje, Logo ERP’ye işlenen hareketleri SQL’den alarak WhatsApp (WhatsApp Business API veya Twilio) ile müşteriye/tedarikçiye gönderir.

---

## 🚀 Ana Özellikler

- 🧠 Logo ERP SQL verisi üzerinden tetiklenebilir anlık/düzenli bildirim
- 🔁 WhatsApp + e‑posta gönderim opsiyonu
- ⏱️ Quartz.NET ile zamanlanmış görevler
- 🧾 Bildirim loglama & gönderim durumu takibi
- 🔐 Kullanıcı tanımlı filtreleme ve şablon desteği

---

## 📂 Proje Yapısı

LogoWhatsappEntegrasyon/
├── Services/ # WhatsApp / Mail / Log servisleri
├── Tasks/ # Zamanlanmış görev tanımları (Quartz)
├── Data/ # SQL sorguları, filtreler, DB modelleri
├── Config/ # appsettings.json / API anahtarları
├── Logging/ # Loglama mekanizmaları
├── Utils/ # Yardımcı sınıflar (şablon, tarih vs.)
└── README.md # Bu dosya

---

## 🔧 Kurulum & Kullanım

1. `appsettings.json` içine:
   - SQL Server bağlantısı
   - WhatsApp API anahtarı (Twilio/Business)
   - SMTP bilgileri (mail için)

2. Görev tanımı yap (`Quartz`):
   JSON/XML veya kod üzerinden filtre, zaman periyodu ve şablon belirle.

3. Servisi başlat:

---

## 📝 Bildirim Yapısı

- SQL’den alınan kayıtlar şablona yerleştirilir (örneğin: “Sipariş X geldi.”)
- WhatsApp mesajı olarak gönderilir; hata varsa loglanır, başarılıysa onaylanır.
- Opsiyonel e‑posta ile yedekleme yapılabilir.

---

## 🧪 Teknik Detaylar

- .NET Core veya .NET 6+
- Quartz.NET ile zamanlama
- WhatsApp için Twilio veya direkt API entegrasyonu
- SMTP tabanlı mail gönderimi
- Gönderim ve hata logları JSON formatında tutulur
- Filter + Template ile esnek yapı

---

## 🛠️ Geliştirici Notları

- `try‑catch` ile her gönderimde hata kontrolü
- Loglama için `Logger.Log(...)` klasörü/servisi
- SQL sorgularını `Data/Queries` içinde tut
- Şablon dosyaları `.tmpl` veya JSON olabilir


---

## 🤝 Katkı

Katkı sağlamak için projeyi forklayabilir ve pull request gönderebilirsiniz.

---

## 📄 Lisans

MIT License

---

## 📬 İletişim

- 👨‍💻 Geliştirici: [@dogukankosan](https://github.com/dogukankosan)  
- 🐞 Suggestions or issues: [Issues sekmesi](https://github.com/dogukankosan/LogoWhatsappEntegrasyon/issues)

---



