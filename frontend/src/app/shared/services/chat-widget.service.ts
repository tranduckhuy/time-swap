import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { MultiLanguageService } from './multi-language.service';

import type { Messages } from '../models/utils/chat-widget.model';

@Injectable({
  providedIn: 'root',
})
export class ChatWidgetService {
  private readonly http = inject(HttpClient);
  private readonly multiLanguageService = inject(MultiLanguageService);

  private readonly apiUrl = environment.apiGptUrl;
  private readonly apiKey = environment.chatGptKey;
  private readonly orgKey = environment.chatGptOrgKey;

  private messagesSignal = signal<Messages[]>([]);
  messages = this.messagesSignal.asReadonly();

  sendMessage(userMessage: string): Observable<any> {
    const requestBody = {
      model: 'gpt-4o-mini',
      messages: [{ role: 'user', content: userMessage }],
      max_tokens: 100,
    };

    return this.http.post(this.apiUrl, requestBody, {
      headers: {
        Authorization: `Bearer ${this.apiKey}`,
        'OpenAI-Organization': this.orgKey,
        'Content-Type': 'application/json',
      },
    });
  }

  sendMessageAndUpdate(userMessage: string): void {
    if (!userMessage.trim()) {
      this.messagesSignal.update((prev) => [
        ...prev,
        {
          text: this.multiLanguageService.getTranslatedLang(
            'chat-widget.empty-message',
          ),
          sender: 'bot',
        },
      ]);
      return;
    }

    this.messagesSignal.update((prev) => [
      ...prev,
      { text: userMessage, sender: 'user' },
    ]);

    this.sendMessage(userMessage).subscribe((response) => {
      const botReply =
        response.choices[0]?.message?.content ||
        this.multiLanguageService.getTranslatedLang(
          'chat-widget.do-not-understand',
        );

      this.messagesSignal.update((prev) => [
        ...prev,
        { text: botReply, sender: 'bot' },
      ]);
    });
  }
}
