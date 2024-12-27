import { AfterViewInit, Component, ElementRef, Renderer2 } from '@angular/core';

import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements AfterViewInit {
  constructor(private router: Router, private el: ElementRef, private renderer: Renderer2) {}
  
  ngAfterViewInit(): void {
    if (this.isHomeRoute()) {
      const mainNavElement = this.el.nativeElement.querySelector('.main-nav');
  
      if (mainNavElement) {
        this.renderer.setStyle(mainNavElement, 'background-color', '#000');
      }
    }
  }
  
  isHomeRoute(): boolean {
    const currentRoute = this.router.url;
    return currentRoute === '' || currentRoute === '/home';
  }
}
