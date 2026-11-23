# finansTakip

C# WinForms ile geliştirilmiş kişisel finans takip uygulaması. Gelir ve giderlerinizi kategorilere ayırarak kaydeder ve bir kontrol panelinde özetler.

## Özellikler

- Kullanıcı kaydı ve girişi
- Kategori yönetimi (gelir/gider türüne göre)
- Gelir ve gider kaydı (tutar, açıklama, tarih)
- Kontrol panelinde (Dashboard) özet görünüm

## Teknolojiler

- C# / .NET Framework 4.7.2 — Windows Forms
- MS SQL Server LocalDB (`expense3.mdf`)
- ADO.NET (`System.Data.SqlClient`)

## Kurulum

1. Depoyu klonlayın ve `finansTakip.sln` dosyasını Visual Studio ile açın.
2. SQL Server LocalDB'yi kurun ve `SQLQueryYeni.sql` betiğini çalıştırarak `users`, `categories`, `income`, `expenses` tablolarını oluşturun.
3. Form dosyalarındaki `stringConnection` değişkenini kendi `.mdf` dosya yolunuza göre güncelleyin.
4. Projeyi derleyip çalıştırın.

## Dosya Yapısı

| Dosya | Açıklama |
|---|---|
| `Program.cs` | Uygulama giriş noktası |
| `Form1.cs` | Giriş ekranı |
| `UyeOlma.cs` | Üye kayıt ekranı |
| `DashboardForm.cs` | Kontrol paneli / özet |
| `IncomeForm.cs`, `ExpensesForm.cs` | Gelir ve gider kayıt ekranları |
| `CategoryForm.cs` | Kategori yönetimi |
| `SQLQueryYeni.sql` | Veritabanı şeması |
