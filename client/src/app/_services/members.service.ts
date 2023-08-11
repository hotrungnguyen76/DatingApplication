import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { Observable, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) {}

  getMembers() {
    if (this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map((members) => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(username: string) {
    const member = this.members.find((m) => m.userName === username);
    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(updatedMember: Member) {
    return this.http.put(this.baseUrl + 'users', updatedMember);
  }

  uploadPhoto(file: any): Observable<any> {
    const formData = new FormData();

    formData.append('file', file, file.name);

    return this.http.post(this.baseUrl + 'users/add-photo', formData);
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}
