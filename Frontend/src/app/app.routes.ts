import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'home',
    loadComponent: () => import('./home/home.page').then((m) => m.HomePage),
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'authenticate',
    loadComponent: () => import('./authenticate/authenticate.page').then( m => m.AuthenticatePage)
  },
  {
    path: 'create-post',
    loadComponent: () => import('./create-post/create-post.page').then( m => m.CreatePostPage)
  },
];
