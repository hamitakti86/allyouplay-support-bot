using SupportBot.Core.Models;

namespace SupportBot.Core.Data;

/// <summary>
/// In-memory knowledge base with the most common scenarios for the Allyouplay.com store.
/// </summary>
public static class IssueTemplateProvider
{
    public static IReadOnlyList<IssueTemplate> CreateDefaultTemplates()
    {
        return new List<IssueTemplate>
        {
            new()
            {
                Category = "digital-key-delay",
                Summary = "Payment is confirmed but the activation key has not arrived yet.",
                Keywords = new[]
                {
                    "key gelmedi", "kod gelmedi", "kod yok", "key yok", "aktivasyon kodu", "cd key",
                    "preorder", "pre-order", "backorder", "erişemedim", "get key", "redeem butonu"
                },
                ReplyTemplate =
                    "Ödemeniz onaylanmış görünüyor ancak {{gameName}} anahtarınız henüz dağıtıma açılmamış olabilir. Çok talep gören oyunlarda anahtarlar ön sipariş veya backorder sürecinde dalgalar hâlinde teslim edilir. Get Key düğmesini birkaç dakika arayla kontrol ederek tekrar deneyebilirsiniz; anahtar hazır olduğunda buton yeşile dönecektir. Bekleme süresi uzarsa size e-posta ile de haber veririz.",
                SuggestedActions = new[]
                {
                    "Hesabınıza giriş yaptıktan sonra \"Kütüphanem\" sekmesinden ilgili siparişi açın.",
                    "Get Key / Anahtarı Al düğmesine tıklayın ve yeni bir anahtar tanımlanıp tanımlanmadığını kontrol edin.",
                    "Buton gri görünüyorsa dağıtım henüz tamamlanmamıştır; 15-20 dakika sonra tekrar deneyin.",
                    "24 saat içinde anahtar ulaşmazsa destek talebinize sipariş numaranızı ekleyin ki stok durumunu manuel kontrol edebilelim."
                },
                FollowUpQuestions = new[]
                {
                    "Sipariş numaranızı paylaşabilir misiniz?",
                    "Oyun kütüphanenizde Get Key düğmesi hangi durumda görünüyor?",
                    "Ön sipariş yaptığınız tarihte ödeme sağlayıcınızda provizyon düşmüş mü gözüküyor?"
                },
                AdditionalNotes = new[]
                {
                    "Geciken anahtarlar için stok ekranında \"awaiting publisher batch\" olup olmadığını kontrol edin.",
                    "VIP müşteriler için eldeki manuel anahtar stoğu varsa önceliklendirin."
                }
            },
            new()
            {
                Category = "payment-confirmed-no-order",
                Summary = "Müşteri ödeme yaptı fakat sipariş listesinde görünmüyor.",
                Keywords = new[]
                {
                    "ödeme yaptım", "para çekildi", "provizyon", "hesabımdan çekildi", "sipariş gözükmüyor",
                    "sipariş oluşmadı", "order görünmüyor", "checkout", "kredi kartı", "hata verdi"
                },
                ReplyTemplate =
                    "Ödemenin bankanız tarafından çekildiğini görüyor olabilirsiniz; bazı sağlayıcılarda provizyon ilk etapta bekleyen işlemler listesinde kalabiliyor. Allyouplay tarafında başarısız olan bir ödeme 1-2 iş günü içinde otomatik olarak iade edilir. Bu süre sonunda provizyon çözümlenmezse bankanıza \"işlem referans kodu\" ile başvurabilirsiniz. Siparişi tekrar denemeden önce tarayıcı önbelleğini temizleyip farklı bir ödeme yöntemi seçmenizi öneririz.",
                SuggestedActions = new[]
                {
                    "Ödeme ekranında aldığınız hata mesajının ekran görüntüsünü paylaşın.",
                    "Bankanızdaki işlem referans kodunu not alın; gerekirse destek ekibi ile paylaşın.",
                    "Sipariş adımlarını tekrar denemeden önce hesabınızda yeterli bakiye olduğundan emin olun.",
                    "Alternatif olarak PayPal veya 3D Secure destekli kart kullanmayı deneyin."
                },
                FollowUpQuestions = new[]
                {
                    "Ödeme tarihi ve saatini paylaşabilir misiniz?",
                    "Hangi ödeme yöntemini tercih ettiniz?",
                    "Sipariş sırasında bir hata kodu gördünüz mü?"
                },
                AdditionalNotes = new[]
                {
                    "Stripe/PayTR panellerinde başarısız işlem var mı kontrol edin.",
                    "Müşteriden PSP referans kodu alındığında charge status'ü eşleştirin."
                }
            },
            new()
            {
                Category = "refund-request",
                Summary = "Müşteri iade veya iptal talep ediyor.",
                Keywords = new[]
                {
                    "iade", "iptal", "refund", "geri ödeme", "paramı geri", "yanlış oyun",
                    "memnun kalmadım", "iptal etmek istiyorum"
                },
                ReplyTemplate =
                    "Alışverişiniz dijital teslimat olduğu için anahtarın kullanılmamış olması şartıyla 14 gün içerisinde iade talebi açabilirsiniz. Lütfen anahtarı hiçbir platformda etkinleştirmediğinizden emin olun. Talebinizi doğruladıktan sonra ödemeniz, alışverişi yaptığınız kanala bağlı olarak 3-5 iş günü içinde geri yansır.",
                SuggestedActions = new[]
                {
                    "Sipariş numaranızı ve iade sebebinizi destek talebinize ekleyin.",
                    "Anahtarı kullandıysanız ilgili platform (Steam, EA App vb.) ekran görüntüsü ile birlikte bildirin.",
                    "İade onaylandığında bankanızdan gelen bilgilendirmeyi takip edin."
                },
                FollowUpQuestions = new[]
                {
                    "Anahtarı herhangi bir platformda etkinleştirdiniz mi?",
                    "İade sebebinizi kısaca paylaşabilir misiniz?",
                    "İade ödemenizi hangi kanaldan almıştınız?"
                },
                AdditionalNotes = new[]
                {
                    "Kullanılan anahtarlar için yayıncı politikasını kontrol edin.",
                    "VIP müşterilerde goodwill iade yapılacaksa finans ekibine bilgi verin."
                }
            },
            new()
            {
                Category = "download-issue",
                Summary = "Müşteri oyunu indiremiyor veya launcher hatası alıyor.",
                Keywords = new[]
                {
                    "indiremiyorum", "download", "launcher", "steam etkinleştirme", "activation error",
                    "redeem", "kod çalışmıyor", "steam hatası", "origin", "ea app", "ubisoft connect"
                },
                ReplyTemplate =
                    "Anahtarı aldıktan sonra etkinleştirme işlemini oyun platformunun istemcisi üzerinden yapmanız gerekiyor. Örneğin Steam için sol alttaki \"+ Oyun Ekle\" > \"Steam'de Ürün Etkinleştir\" akışını takip edebilirsiniz. Kodun çalışmaması durumunda yazım hatası olmadığından ve doğru bölge/versiyonu kullandığınızdan emin olun.",
                SuggestedActions = new[]
                {
                    "Platform istemcisini yönetici olarak çalıştırın ve güncel sürümde olduğundan emin olun.",
                    "Anahtarın tire ve rakamlarını eksiksiz kopyaladığınızdan emin olun; manuel yazmanız önerilir.",
                    "Sorun devam ederse hata ekran görüntüsünü destek ekibine gönderin."
                },
                FollowUpQuestions = new[]
                {
                    "Hangi launcher veya platformu kullanıyorsunuz?",
                    "Herhangi bir hata kodu görüyor musunuz?",
                    "Oyunu daha önce farklı bir bölgeden satın alıp etkinleştirmiş miydiniz?"
                },
                AdditionalNotes = new[]
                {
                    "Bölgesel kısıtlamalar için yayıncıya bilet açılması gerekebilir.",
                    "Gerekirse manuel olarak yeni anahtar atanması için stok kontrolü yapın."
                }
            },
            new()
            {
                Category = "account-access",
                Summary = "Kullanıcı hesabına giriş yapamıyor veya güvenlik kilidine takılıyor.",
                Keywords = new[]
                {
                    "hesabım", "giriş yapamıyorum", "login", "2fa", "şifre", "password reset",
                    "güvenlik kilidi", "account locked", "e-posta gelmedi"
                },
                ReplyTemplate =
                    "Hesabınıza erişemediğinizi görüyorum. Güvenlik için şifre sıfırlama bağlantıları kısa süreli geçerlidir; gelen kutunuzda göremiyorsanız spam klasörünü kontrol edin. 2 adımlı doğrulama aktifse doğrulama uygulamanızdaki kodu girmeniz gerekir. Erişim alamazsanız destek talebinize kayıtlı e-posta adresinizi ekleyin, ekibimiz 24 saat içinde hesabınızı doğrulayıp manuel reset sağlayacaktır.",
                SuggestedActions = new[]
                {
                    "Hesap kurtarma sayfasından \"Şifremi Unuttum\" akışını başlatın.",
                    "Spam ve Gereksiz klasörlerini kontrol edin.",
                    "Hâlâ giriş yapamıyorsanız destek ekibine 2FA kodu üretemediğinizi belirtin."
                },
                FollowUpQuestions = new[]
                {
                    "Hata mesajı tam olarak neydi?",
                    "Şifre sıfırlama e-postası ulaştı mı?",
                    "Hesabınızda 2 adımlı doğrulama açık mı?"
                },
                AdditionalNotes = new[]
                {
                    "Çok sayıda başarısız giriş varsa geçici IP engeli olabilir, firewall'da kontrol edin.",
                    "Kilitli hesaplar için kimlik doğrulaması isteyin."
                }
            },
            new()
            {
                Category = "general-question",
                Summary = "Önceki şablonlara uymayan genel bir soru.",
                Keywords = new[] { "merhaba", "selam", "soru", "bilgi", "yardım" },
                ReplyTemplate =
                    "Size yardımcı olmak için buradayım! Siparişiniz, anahtar teslimatı veya teknik destek ile ilgili detayları paylaşırsanız birlikte hızlıca çözebiliriz.",
                SuggestedActions = new[]
                {
                    "Yaşadığınız durumu mümkün olduğunca detaylı yazın.",
                    "Eğer varsa sipariş numaranızı ekleyin ki hesabınızı kontrol edebilelim.",
                    "Çözüm bulamazsanız canlı destek ekibimize talep oluşturabilirsiniz."
                },
                FollowUpQuestions = new[]
                {
                    "Sizi nasıl hitap edelim?",
                    "Bu konu hangi siparişle ilgili?",
                    "Daha önce buna benzer bir sorun yaşadınız mı?"
                },
                AdditionalNotes = new[]
                {
                    "Konu belirlenemezse canlı temsilci devreye alın.",
                    "CRM kaydında ilgili etiketleri boş bırakmayın."
                }
            }
        };
    }
}
