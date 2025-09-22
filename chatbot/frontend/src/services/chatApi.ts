export interface ChatMessagePayload {
  message: string;
  orderNumber?: string;
  locale?: string;
}

export interface ChatbotReply {
  reply: string;
  category: string;
  confidence: number;
  suggestedActions: string[];
  followUpQuestions: string[];
  additionalNotes: string[];
}

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5050';

export async function sendMessage(payload: ChatMessagePayload): Promise<ChatbotReply> {
  const response = await fetch(`${API_BASE_URL}/api/chat`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      message: payload.message,
      orderNumber: payload.orderNumber,
      locale: payload.locale ?? 'tr-TR'
    })
  });

  if (!response.ok) {
    const description = await response.text();
    throw new Error(description || 'Chatbot servisine ulaşılamadı.');
  }

  return response.json() as Promise<ChatbotReply>;
}
