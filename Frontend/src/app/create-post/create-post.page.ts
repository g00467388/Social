import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonContent, IonHeader, IonTitle, IonToolbar, IonLabel, IonInput, IonButton, IonButtons, IonIcon, IonFooter, IonCardHeader, IonCardSubtitle, IonCard, IonCardTitle, IonCardContent, IonItem, ToastController } from '@ionic/angular/standalone';
import { HttpClient } from '@angular/common/http';
import { Post } from '../models/post';
import { Endpoints } from '../helpers/endpoints';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ThemeService } from '../services/theme-service';
import { IonTextarea } from '@ionic/angular/standalone'

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.page.html',
  styleUrls: ['./create-post.page.scss'],
  standalone: true,
  imports: [IonItem, IonTextarea, IonCardContent, IonCardTitle, IonCard, IonCardSubtitle, IonCardHeader, IonFooter, IonInput, IonLabel, IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, FormsModule, IonButton, IonButtons, IonIcon, RouterLink, RouterLinkActive]
})
export class CreatePostPage {
  themeService :ThemeService = ThemeService.getInstance()
  post :Post = new Post(); 
  
  constructor(private httpClient :HttpClient,
    private toastController: ToastController,
    private router: Router
) { }

  ngOnInit() {
  }

  async submit() {
  console.log(this.post);

  this.httpClient.post(Endpoints.REQUEST_POST, this.post, {
    withCredentials: true,
    observe: 'response'
  }).subscribe({
    next: async (response) => {
      const location = response.headers.get('Location');

      if (response.status === 201) {
        await this.showToast('Post created successfully!', 'success');
      } else {
        console.warn('Invalid CreatedAtAction response', response);
        await this.showToast('Warning: Post may not have been created correctly.', 'warning');
      }
    },
    error: async (err) => {
      console.error('Request failed', err);
      await this.showToast('Error: Failed to create post.', 'danger');
    }
    
  });
        this.router.navigate(['/home'])

}

async showToast(message: string, warnLevel : string) {
  const toast = await this.toastController.create({
    message,
    duration: 3000,
    position: 'bottom',
    color: warnLevel
  });
  await toast.present();
}
  toggleTheme() {
    this.themeService.toggleTheme()
  }
 
}
