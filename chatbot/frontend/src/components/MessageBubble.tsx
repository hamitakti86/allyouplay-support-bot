import type { ChatbotReply } from '../services/chatApi';

type Sender = 'user' | 'bot';

export interface MessageBubbleProps {
  sender: Sender;
  text: string;
  meta?: Pick<ChatbotReply, 'suggestedActions' | 'followUpQuestions' | 'category' | 'confidence'>;
}

export function MessageBubble({ sender, text, meta }: MessageBubbleProps) {
  const isBot = sender === 'bot';

  return (
    <div className={`message-bubble ${isBot ? 'bot' : 'user'}`}>
      {isBot && meta && (
        <div className="message-header">
          <span className="category">{mapCategory(meta.category)}</span>
          <span className="confidence">{Math.round(meta.confidence * 100)}% güven</span>
        </div>
      )}
      <p className="message-text">{text}</p>
      {isBot && meta && meta.suggestedActions.length > 0 && (
        <div className="message-section">
          <h4>Önerilen adımlar</h4>
          <ul>
            {meta.suggestedActions.map((action) => (
              <li key={action}>{action}</li>
            ))}
          </ul>
        </div>
      )}
      {isBot && meta && meta.followUpQuestions.length > 0 && (
        <div className="message-section">
          <h4>Bizimle paylaşabileceğiniz bilgiler</h4>
          <ul>
            {meta.followUpQuestions.map((question) => (
              <li key={question}>{question}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
}

function mapCategory(category: string) {
  switch (category) {
    case 'digital-key-delay':
      return 'Anahtar teslimatı gecikti';
    case 'payment-confirmed-no-order':
      return 'Ödeme görünüyor, sipariş oluşmadı';
    case 'refund-request':
      return 'İade talebi';
    case 'download-issue':
      return 'İndirme/aktivasyon sorunu';
    case 'account-access':
      return 'Hesaba erişemiyorum';
    case 'general-question':
      return 'Genel soru';
    default:
      return 'Destek bilgisi';
  }
}
