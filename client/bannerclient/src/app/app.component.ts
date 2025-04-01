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

  bannerTitle = '';
  bannerbackgroundColor = '#FFFFF0';
  bannerdescriptionColor = '';
  bannerbuttonText = 'click me!';
  bannerbuttontextColor = 'white';
  buttonbackgroundColor = '#FF7F50';
  bannerDescription = 'hello world!';
  logoUrl = 'https://app.socxo.io/assets/images/banner-placeholder-logo.svg';
  isLoading = false;
  

  constructor(private bannerService:BannerService) {}

  ngOnInit() {}
  
  generateBanner(){
    //load the loader for the banner generation
    this.isLoading = true;
    this.bannerService.generateBannerWithAI(this.bannerTitle).subscribe((response) => {
      console.log('Banner generated:', response);
      try{
       const cleanJson = response.data.replace(/\\"/g, '"');
       var BannerData = JSON.parse(cleanJson);
       console.log("BannerData:",BannerData);
       
       if(BannerData){
        this.bannerDescription = BannerData.bannerDescription;
        this.bannerbackgroundColor = BannerData.backgroundColor;
        this.bannerbuttonText = BannerData.ButtonText;
        this.bannerbuttontextColor = BannerData.ButtonTextColor;
        this.bannerdescriptionColor = BannerData.descriptionColor;
        this.buttonbackgroundColor = BannerData.ButtonBackgroundColor;
        this.logoUrl = BannerData.logoUrl;
       }
      }
      catch(e){
         console.log("Error in parsing JSON:",e);
      }

      this.isLoading = false;
    } );
  }


}
