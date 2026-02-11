import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PostService } from './post-service.service';

describe('PostService', () => {
  let service: PostService;
  let httpMock: HttpTestingController;
  const baseUrl = 'https://localhost:7002/api/Post';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PostService]
    });
    service = TestBed.inject(PostService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch post by id', () => {
    const mockPost = { id: '1', title: 'Test', content: 'Test content' };
    service.getPostById(1).subscribe(post => {
      expect(post).toEqual(mockPost);
    });
    const req = httpMock.expectOne(`${baseUrl}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockPost);
  });

  it('should fetch all posts', () => {
    const mockPosts = [
      { id: '1', title: 'Test1', content: 'Content1' },
      { id: '2', title: 'Test2', content: 'Content2' }
    ];
    service.getAllPosts().subscribe(posts => {
      expect(posts.length).toBe(2);
      expect(posts).toEqual(mockPosts);
    });
    const req = httpMock.expectOne(baseUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockPosts);
  });
});