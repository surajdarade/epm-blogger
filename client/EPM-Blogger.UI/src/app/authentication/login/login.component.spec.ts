import { TestBed, ComponentFixture, fakeAsync, tick } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

describe('LoginComponent', () => {
  let fixture: ComponentFixture<LoginComponent>;
  let component: LoginComponent;
  let httpMock: HttpTestingController;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      // 👇 Import the standalone component directly
      imports: [
        LoginComponent,
        HttpClientTestingModule, // 👈 Required for HTTP interception
        RouterTestingModule.withRoutes([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    httpMock = TestBed.inject(HttpTestingController);

    spyOn(router, 'navigate'); // spy on navigation

    fixture.detectChanges(); // very important to trigger ngOnInit and setup
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  
  //   // Arrange
  //   component.loginData = {
  //     UsernameOrEmail: 'test@example.com',
  //     password: 'testpass'
  //   };

  //   // Act
  //   component.onSubmit();

  //   // Assert: HTTP request is expected
  //   const req = httpMock.expectOne('https://localhost:7002/api/auth/login');
  //   expect(req.request.method).toBe('POST');

  //   const mockResponse = {
  //     accessToken: 'mockAccessToken',
  //     refreshToken: 'mockRefreshToken',
  //     user: { id: 1, name: 'Test User' }
  //   };

  //   req.flush(mockResponse); // send mock response
  //   tick(); // flush all observables and promises

  //   // Check localStorage
  //   expect(localStorage.getItem('accessToken')).toBe('mockAccessToken');
  //   expect(localStorage.getItem('refreshToken')).toBe('mockRefreshToken');
  //   expect(localStorage.getItem('user')).toBe(JSON.stringify(mockResponse.user));
  //   expect(localStorage.getItem('isLoggedIn')).toBe('true');

  //   // Check navigation
  //   expect(router.navigate).toHaveBeenCalledWith(['/']);
  // }));
  it('debug: log when form is submitted', () => {
  component.loginData = {
    UsernameOrEmail: 'test@example.com',
    password: 'testpass'
  };
  spyOn(component['http'], 'post').and.callThrough();
  component.onSubmit();

  console.log('Submitted.'); // See if it gets here
});
});
