import { Component, Input, Output, EventEmitter, computed } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent {
  @Input() currentPage: number = 1; 
  @Input() totalJobs: number = 1; 
  @Input() limit: number = 1; 
  totalPages = computed<number>(() => Math.round(Math.ceil(this.totalJobs / this.limit)))
  @Output() pageChange: EventEmitter<number> = new EventEmitter<number>();

  // Get visible page numbers (limit to 3)
  get visiblePages(): number[] {
    const pages: number[] = [];
    const start = Math.max(1, this.currentPage - 1); // Start at currentPage - 1
    const end = Math.min(this.totalPages(), this.currentPage + 1); // End at currentPage + 1

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }

  // Emit the new page number when clicked
  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages() && this.currentPage !== page) {
      this.pageChange.emit(page);
    }
  }
}
