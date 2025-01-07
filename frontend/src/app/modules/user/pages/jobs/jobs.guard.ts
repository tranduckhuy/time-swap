import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const jobDetailCanActivate: CanActivateFn = (activatedRouteSnapshot) => {
  const router = inject(Router);
  
  const job = activatedRouteSnapshot.data['job'];
  console.log(activatedRouteSnapshot);

  if (!job) {
      router.navigate(['/not-found']);
      return false;
  }
  
  return true;
};
