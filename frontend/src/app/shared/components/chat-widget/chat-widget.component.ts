import {
  AfterViewInit,
  Component,
  effect,
  ElementRef,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

import { ChatWidgetService } from '../../services/chat-widget.service';

import { fadeInOut } from '../../models/utils/chat-widget.model';

@Component({
  selector: 'app-chat-widget',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './chat-widget.component.html',
  styleUrl: './chat-widget.component.css',
  animations: [fadeInOut],
})
export class ChatWidgetComponent implements OnInit, AfterViewInit {
  @ViewChild('chatBody') chatBody!: ElementRef<HTMLDivElement>;

  private readonly chatWidgetService = inject(ChatWidgetService);
  private readonly fb = inject(FormBuilder);

  chatForm!: FormGroup;

  isChatOpen = signal(false);
  messages = this.chatWidgetService.messages;

  constructor() {
    effect(() => {
      this.messages();
      this.scrollToBottom();
    });
  }

  ngOnInit(): void {
    this.initForm();
  }

  ngAfterViewInit(): void {
    this.scrollToBottom();
  }

  initForm() {
    this.chatForm = this.fb.group({
      message: [''],
    });
  }

  toggleChat() {
    this.isChatOpen.set(!this.isChatOpen());
    setTimeout(() => this.scrollToBottom(), 100);
  }

  sendMessage() {
    const message = this.chatForm.value.message?.trim() || '';

    this.chatWidgetService.sendMessageAndUpdate(message);
    this.chatForm.reset();
  }

  private scrollToBottom() {
    setTimeout(() => {
      if (this.chatBody) {
        const chatBodyEl = this.chatBody.nativeElement;
        chatBodyEl.lastElementChild?.scrollIntoView({ behavior: 'smooth' });
      }
    }, 100);
  }
}
