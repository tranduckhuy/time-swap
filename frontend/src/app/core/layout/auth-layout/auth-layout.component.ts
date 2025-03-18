import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { ChatWidgetComponent } from '../../../shared/components/chat-widget/chat-widget.component';

@Component({
  selector: 'app-auth-layout',
  standalone: true,
  imports: [RouterOutlet, ChatWidgetComponent],
  templateUrl: './auth-layout.component.html',
})
export class AuthLayoutComponent {}
