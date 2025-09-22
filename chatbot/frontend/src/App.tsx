import { FormEvent, useMemo, useState } from 'react';
import { MessageBubble } from './components/MessageBubble';
import type { ChatbotReply } from './services/chatApi';
import { sendMessage } from './services/chatApi';
import './App.css';

type ConversationMessage = {
  id: number;
  sender: 'user' | 'bot';
  text: string;
  meta?: Pick<ChatbotReply, 'suggestedActions' | 'followUpQuestions' | 'category' | 'confidence'>;
};

let messageId = 0;

export default function App() {
  const [messages, setMessages] = useState<ConversationMessage[]>(() => [
    {
      id: ++messageId,
      sender: 'bot',
      text: 'Merhaba! Allyouplay destek botuna hoş geldiniz. Siparişinizle ilgili yaşadığınız sorunu bir iki cümle ile anlatın, birlikte çözelim.'
    }
  ]);
  const [messageInput, setMessageInput] = useState('');
  const [orderNumber, setOrderNumber] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const canSend = useMemo(() => messageInput.trim().length > 2 && !loading, [messageInput, loading]);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!canSend) {
      return;
    }

    const message = messageInput.trim();
    const order = orderNumber.trim() || undefined;

    setMessages((prev) => [
      ...prev,
      { id: ++messageId, sender: 'user', text: message }
    ]);
    setMessageInput('');
    setError(null);
    setLoading(true);

    try {
      const response = await sendMessage({ message, orderNumber: order });
      setMessages((prev) => [
        ...prev,
        {
          id: ++messageId,
          sender: 'bot',
          text: response.reply,
          meta: {
            suggestedActions: response.suggestedActions,
            followUpQuestions: response.followUpQuestions,
            category: response.category,
            confidence: response.confidence
          }
        }
      ]);
    } catch (err) {
      console.error(err);
      setError(err instanceof Error ? err.message : 'Bir hata oluştu.');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="app-shell">
      <header>
        <h1>Allyouplay Destek Botu</h1>
        <p>
          Dijital oyun alışverişlerinizde yaşadığınız sorunları saniyeler içinde çözüme kavuşturmak için buradayız. Sorununuzu yazın,
          botumuz ön tanımlı senaryolara göre size yol göstersin.
        </p>
      </header>

      <main>
        <div className="chat-window">
          {messages.map((message) => (
            <MessageBubble key={message.id} sender={message.sender} text={message.text} meta={message.meta} />
          ))}
        </div>

        <form className="chat-form" onSubmit={handleSubmit}>
          <label className="field">
            <span>Sipariş Numaranız (opsiyonel)</span>
            <input
              type="text"
              placeholder="AYP-123456"
              value={orderNumber}
              onChange={(event) => setOrderNumber(event.target.value)}
            />
          </label>

          <label className="field">
            <span>Mesajınız</span>
            <textarea
              placeholder="Örn: Helldivers aldım ama aktivasyon kodu gelmedi."
              value={messageInput}
              onChange={(event) => setMessageInput(event.target.value)}
              rows={4}
            />
          </label>

          {error && <div className="error-banner">{error}</div>}

          <div className="actions">
            <button type="submit" disabled={!canSend}>
              {loading ? 'Yanıt bekleniyor…' : 'Mesajı Gönder'}
            </button>
          </div>
        </form>
      </main>
    </div>
  );
}
