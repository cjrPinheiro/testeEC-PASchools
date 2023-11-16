import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { School } from '@app/models/school';
import { Address } from '@app/models/address';
import { Coordinate } from '@app/models/coordinate';
import { CoordinateInterface } from '@app/models/coordinateInterface';
import { Route } from '@app/models/route';
import { SyncResponse } from '@app/models/syncResponse';

import { environment } from '@environments/environment';


@Injectable()
export class HomeService {

  constructor(private http: HttpClient) { }

  baseURL = environment.baseApiUrl + '/main';

  public syncSchools(limit: number): Observable<SyncResponse>{
    return this.http.get<SyncResponse>(`${this.baseURL}/UpdateSchoolDatabase?rowsLimit=${limit}`);
  }

  public postGetCoordinates(address: Address) : Observable<Coordinate> {
    return this.http.post<Coordinate>(`${this.baseURL}/Coordinates`, address);
  }

  public getSchoolsOrdered(origin: Coordinate) : Observable<School[]> {
    return this.http.get<School[]>(`${this.baseURL}/SchoolsOrdered?lat=${origin.lat}&lng=${origin.lng}`);
  }

  public postGetRoute(origin: Coordinate, destination: Coordinate) : Observable<Route> {
    return this.http.post<Route>(`${this.baseURL}/Route`, { Origin: origin, Destination: destination });
  }

}
