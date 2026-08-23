import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MsalService, MsalBroadcastService } from '@azure/msal-angular';
import { InteractionStatus } from '@azure/msal-browser';
import { filter, take } from 'rxjs/operators';
import { environment } from '../../environments/environment';

interface AccessTokenPayload {
  roles?: string[];
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  forecasts: any[] = [];
  adminData: any = null;
  error = '';
  adminError = '';
  isAdmin = false;

  constructor(
    private http: HttpClient,
    private msalService: MsalService,
    private msalBroadcastService: MsalBroadcastService
  ) {}

  ngOnInit(): void {
    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        take(1)
      )
      .subscribe(() => {
        const account = this.msalService.instance.getAllAccounts()[0];
        if (account) {
          this.msalService.instance.setActiveAccount(account);
          this.setRoleFlags();
        }
      });

    this.http.get<any[]>(environment.apiConfig.endpoint).subscribe({
      next: (data) => (this.forecasts = data),
      error: (err) => (this.error = `${err.status}: ${err.message}`)
    });
  }

  private async setRoleFlags(): Promise<void> {
    const account = this.msalService.instance.getActiveAccount();
    if (!account) {
      return;
    }

    try {
      const result = await this.msalService.instance.acquireTokenSilent({
        scopes: [environment.apiConfig.scope],
        account
      });

      const payload: AccessTokenPayload = JSON.parse(
        atob(result.accessToken.split('.')[1])
      );

      this.isAdmin = payload.roles?.includes('Admin') ?? false;
    } catch (err) {
      console.error('Failed to acquire token for role check', err);
      this.isAdmin = false;
    }
  }

  callAdminEndpoint(): void {
    this.http.get(`${environment.apiConfig.endpoint}/admin`).subscribe({
      next: (data) => (this.adminData = data),
      error: (err) => (this.adminError = `${err.status}: ${err.message}`)
    });
  }
}