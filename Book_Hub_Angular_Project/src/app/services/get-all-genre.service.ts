import { Injectable ,signal, ɵgetClosestComponentName} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Constant } from '../constants/constants';

@Injectable({
  providedIn: 'root'
})
export class GetAllGenreService {

  incomingData = signal<any[]>([]);

  constructor(private http: HttpClient) { }

  // Method to fetch data from API
  fetchDatagenre(): void {
   // 'https://localhost:7251/api/Admin/GetGenre'
    this.http.get<any>(Constant.BASE_URI+Constant.Get_All_Genre).subscribe(
      (result) => {
        // Check if result has the "$values" array and assign it to incomingData
        if (result && result.$values && Array.isArray(result.$values)) {
          this.incomingData.set(result.$values);  // Set the incoming data to the signal
        } else {
          console.error('Invalid data structure received from API');
        }
      },
      (error) => {
        console.error('Error fetching data:', error);
        this.incomingData.set([]);  // Clear the data in case of error
      }
    );
  }
}
