# Allyouplay Destek Chatbotu

Bu depo, Allyouplay.com oyun mağazasında yaşanabilecek yaygın müşteri sorunlarına otomatik yanıt üreten bir destek botu örneğini içerir. Proje iki ana bileşenden oluşur:

- **ASP.NET Core Minimal API** (`chatbot/backend`) – Müşteri mesajlarını anahtar kelime ve senaryo temelli kurallarla analiz ederek yanıtlayan servis.
- **React + Vite arayüzü** (`chatbot/frontend`) – Müşterilerin chatbot ile konuşabileceği modern bir sohbet arayüzü.

## Özellikler

- Dijital anahtar gecikmesi, ödeme gözükmeme, iade talebi, indirme/aktivasyon hatası ve hesap erişimi gibi senaryolar için hazır yanıtlar.
- Her yanıtla birlikte önerilen adımlar, takip soruları ve botun güven skoru.
- Sipariş numarası desteği ve oyun adı tespiti ile kişiselleştirilmiş cevaplar.
- Swagger/OpenAPI dokümantasyonu olan REST servisi ve React tabanlı bir istemci.

## Geliştirme ortamı gereksinimleri

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download) veya daha yenisi
- [Node.js 18+](https://nodejs.org/en/download) (Vite geliştirme sunucusu için)
- npm veya pnpm/yarn gibi bir paket yöneticisi

## Backend'i çalıştırma

```bash
cd chatbot/backend
# Bağımlılıkları indirin
dotnet restore

# API'yi başlatın (varsayılan olarak https://localhost:5001 / http://localhost:5000)
dotnet run --project src/SupportBot.Api/SupportBot.Api.csproj
```

Sunucu açıldığında `https://localhost:5001/swagger` adresinden Swagger arayüzüne erişebilir ve `/api/chat` uç noktasını test edebilirsiniz.

### Örnek istek

```http
POST /api/chat HTTP/1.1
Host: localhost:5000
Content-Type: application/json

{
  "message": "Helldivers aldım ancak key gelmedi",
  "orderNumber": "AYP-123456"
}
```

### Örnek yanıt

```json
{
  "reply": "Ödemeniz onaylanmış görünüyor ancak Helldivers II anahtarınız henüz dağıtıma açılmamış olabilir...",
  "category": "digital-key-delay",
  "confidence": 0.73,
  "suggestedActions": ["Hesabınıza giriş yaptıktan sonra ..."],
  "followUpQuestions": ["Sipariş numaranızı paylaşabilir misiniz?"],
  "additionalNotes": ["Geciken anahtarlar için stok ekranında ..."]
}
```

## Frontend'i çalıştırma

```bash
cd chatbot/frontend
npm install

# API için varsayılan URL'yi değiştirmek isterseniz
# (örn. backend http://localhost:5000 üzerinde ise)
# .env dosyası oluşturun ve şu değişkeni ekleyin:
# VITE_API_BASE_URL=http://localhost:5000

npm run dev
```

Komut başarılı olduğunda Vite geliştirme sunucusu `http://localhost:5173` adresinde açılır. Sohbet arayüzü üzerinden mesaj gönderdiğinizde backend API'sine `POST /api/chat` isteği yapılır.

## Dağıtım notları

- Backend herhangi bir ASP.NET Core destekli sunucuya (IIS, Azure App Service, Docker vb.) yayınlanabilir.
- Frontend statik dosya olarak `npm run build` komutu ile üretilebilir. Çıktı `dist/` klasöründe oluşur ve CDN ya da herhangi bir statik sunucuya aktarılabilir.
- CORS yapılandırması varsayılan olarak tüm origin'leri kabul eder; canlı ortama alırken domain bazında kısıtlama yapmanız önerilir.

## Geliştirme için öneriler

- Yeni senaryolar eklemek için `SupportBot.Core/Data/IssueTemplateProvider.cs` dosyasına ek şablonlar tanımlayabilirsiniz.
- Daha gelişmiş sınıflandırma ihtiyacınız olursa `IssueResponseGenerator` servisini genişleterek OpenAI, Azure AI veya kendi ML modelinizi entegre edebilirsiniz.
- Frontend tarafında, bot yanıtlarını saklamak veya canlı operatöre yönlendirmek için ek butonlar ve API çağrıları ekleyebilirsiniz.
