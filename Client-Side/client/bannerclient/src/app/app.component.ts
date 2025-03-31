import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BannerService } from '../core/bannerService';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,CommonModule,FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  bannerTitle:string = '';
  bannerDescription = 'Banner description';
  bannerButton = 'click me!';
  isLoading = false;

  constructor(private bannerService:BannerService) {}

  ngOnInit() {}
  
  generateBanner(){
    //load the loader for the banner generation
    this.isLoading = true;
    this.bannerService.generateBannerWithAI(this.bannerTitle).subscribe((response) => {
      console.log('Banner generated:', response);
      this.isLoading = false;
    } );
  }


}
