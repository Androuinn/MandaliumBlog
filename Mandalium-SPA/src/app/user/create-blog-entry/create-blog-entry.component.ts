import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/Writer';
import { Topic } from 'src/app/_models/Topic';
import { BlogService } from 'src/app/_services/blog.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'src/app/_services/auth.service';
import { environment } from 'src/environments/environment';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { PhotoService } from 'src/app/_services/photo.service';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { Photo } from 'src/app/_models/Photo';

@Component({
  selector: 'app-create-blog-entry',
  templateUrl: './create-blog-entry.component.html',
  styleUrls: ['./create-blog-entry.component.css'],
})
export class CreateBlogEntryComponent implements OnInit {
  topics: Topic[];
  newTopic: Topic = {};
  innerTextHtml: string;
  createBlogPost: FormGroup;
  file: File;
  createNewTopic = false;
  blogEntry: BlogEntry;
  entryId: number;
  photos: Photo[];
  addPhotos = false;
  choosePhotos = false;
  pagination: Pagination;

  constructor(
    private blogService: BlogService,
    formBuilder: FormBuilder,
    private router: Router,
    private alertify: AlertifyService,
    private http: HttpClient,
    public authService: AuthService,
    private photoService: PhotoService
  ) {
    this.createBlogPost = formBuilder.group({
      id: Number,
      headline: ['', [Validators.required, Validators.maxLength(200)]],
      subHeadline: ['', [Validators.required, Validators.maxLength(500)]],
      innerTextHtml: ['', Validators.required],
      photoUrl: ['', Validators.required],
      userId: this.authService.decodedToken.nameid,
      topicId: Number,
      writerEntry: Boolean,
    });
  }

  ngOnInit() {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 5,
      totalItems: 1,
      totalPages: 1
    };
    this.getPhotos();
    this.getTopics();
    this.blogService.currentBlogEntry.subscribe((entry) => (this.entryId = entry));

    if (this.entryId !== 0) {
      this.blogService.getBlogEntry(this.entryId).subscribe((entry: BlogEntry) => {
        this.blogEntry = entry;
        console.log(this.blogEntry);
        this.createBlogPost.setValue({
          id: this.blogEntry.id,
          headline: this.blogEntry.headline,
          subHeadline: this.blogEntry.subHeadline,
          innerTextHtml: this.blogEntry.innerTextHtml,
          photoUrl: this.blogEntry.photoUrl.split('/').reverse().slice(0, 3).reverse().join('/').split('.').slice(0, 1).join(''),
          userId: this.authService.decodedToken.nameid,
          topicId: this.blogEntry.topicId,
          writerEntry: this.blogEntry.writerEntry,
        });
      });
    }
  }

  onFileChanged(event: any) {
    this.file = event.target.files[0];
  }
//#region photos
  postPhoto() {
    const formData: FormData = new FormData();
    formData.append('file', this.file, this.file.name);
    formData.append('writerId', this.authService.decodedToken.nameid);
    console.log(formData);
    if (this.file != null) {
      return this.photoService.postPhoto(formData).subscribe(
        (res) => {
          let url = JSON.stringify(res);
          url = url.split('"').join('');
          this.alertify.success('fotoğraf yükleme başarılı');
          this.createBlogPost.patchValue({photoUrl: res});
          this.getPhotos();
        },
        (error) => {
          console.error(error);
          this.alertify.error(error);
        }
      );
    }
  }


  getPhotos() {
    this.photoService.getPhotos(this.pagination.currentPage, this.pagination.itemsPerPage, false, this.authService.decodedToken.nameid)
    .subscribe((res: PaginatedResult<Photo[]>) => {
      this.photos = (res.result);
      this.pagination = res.pagination;
    },
    error => {
      this.alertify.error(error);
      console.log(error);
    });
   }

   //#endregion

  getTopics() {
    this.blogService.getTopics().subscribe(
      (res: Topic[]) => {
        this.topics = res;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  createOrUpdatePost() {
    if (this.createBlogPost.valid) {
      // update
      if (this.createBlogPost.get('id').value >= 1) {
        this.blogService.updateBlogEntry(this.createBlogPost.value).subscribe(
          () => {
            this.alertify.success('başarılı');
            this.router.navigate(['/']);
          },
          (error) => {
            this.alertify.error(error);
          }
        );
      } else {
        // create
        this.blogService.saveBlogEntry(this.createBlogPost.value).subscribe(
          () => {
            this.alertify.success('başarılı');
            this.router.navigate(['/']);
          },
          (error) => {
            this.alertify.error(error);
            console.log(error);
          }
        );
      }
    }
  }

  addContent(content: string) {
    document.getElementById('textarea').innerText += content;
    this.innerTextHtml += content;
  }

  getInnerHtml(newValue: string) {
    this.innerTextHtml = newValue;
  }


  createTopic() {
    this.blogService.saveTopic(this.newTopic).subscribe(
      (res) => {
        this.createNewTopic = !this.createNewTopic;
        this.alertify.success(this.newTopic.topicName + ' Başarıyla eklendi');
        this.getTopics();
      },
      (error) => {
        this.alertify.error(error);
        console.log(error);
      }
    );
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getPhotos();
  }
}
