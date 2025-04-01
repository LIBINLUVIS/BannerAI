import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BannerService {
  private apiUrl = 'https://localhost:44370/Banner'; // replace with your API endpoint

  constructor(private http: HttpClient) { }

  // Get banner data
  getBannerData(): Observable<any> {
    return this.http.get(`${this.apiUrl}/banner`);
  }

  // Create banner
  createBanner(bannerData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/banner`, bannerData);
  }

  // Generate banner with AI
  generateBannerWithAI(bannerTitle: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/generatebanner`, {
        params: { BannerTittle: bannerTitle }
    });
  }
}