import { Component, OnInit } from '@angular/core';
import { Writer } from 'src/app/_models/Writer';
import { Topic } from 'src/app/_models/Topic';
import { BlogService } from 'src/app/_services/blog.service';
import { WriterTopic } from 'src/app/_models/WriterTopic';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from 'src/app/_services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-create-blog-entry',
  templateUrl: './create-blog-entry.component.html',
  styleUrls: ['./create-blog-entry.component.css']
})
export class CreateBlogEntryComponent implements OnInit {
  writers: Writer[];
  topics: Topic[];
  newTopic: Topic = {};
  innerTextHtml: string;
  createBlogPost: FormGroup;
  file: File;
  createNewTopic = false;


  constructor(
    private blogService: BlogService,
    formBuilder: FormBuilder,
    private router: Router,
    private alertify: AlertifyService,
    private http: HttpClient,
    public authService: AuthService
  ) {
    this.createBlogPost = formBuilder.group({
      headline: ['', [Validators.required, Validators.maxLength(200)]],
      subHeadline: ['', [Validators.required, Validators.maxLength(500)]],
      innerTextHtml: ['', Validators.required],
      photoUrl: ['', Validators.required],
      writerId: Number,
      topicId: Number,
      writerEntry: Boolean
    });
  }

  ngOnInit() {
    this.getTopicsAndWriters();
  }

  onFileChanged(event: any) {
    this.file = event.target.files[0];
  }

  postPhoto() {
    const formData: FormData = new FormData();
    formData.append('file', this.file, this.file.name);
    console.log(formData);
    if (this.file != null) {
      return this.http
        .post(environment.apiUrl + 'photos', formData)
        .subscribe(
          res => {
            let url = JSON.stringify(res);
            url = url.replace('"', '');
            this.alertify.success('fotoğraf yükleme başarılı');
            this.createBlogPost.controls.photoUrl.setValue(url);
          },
          error => {
            console.error(error);
            this.alertify.error(error);
          }
        );
    }
  }

  getTopicsAndWriters() {
    this.blogService.getTopicsAndWriters().subscribe(
      (res: WriterTopic) => {
        this.writers = res.writers;
        this.topics = res.topics;
      },
      error => {
        console.error(error);
      }
    );
  }

  createPost() {
    if (this.createBlogPost.valid) {
      this.blogService.saveBlogEntry(this.createBlogPost.value).subscribe(
        () => {
          this.alertify.success('başarılı');
          this.router.navigate(['/']);
        },
        error => {
          console.error(error);
        }
      );
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
    this.blogService.saveTopic(this.newTopic).subscribe(res => {
      this.createNewTopic = !this.createNewTopic;
      this.getTopicsAndWriters();
      this.alertify.success(this.newTopic.topicName + ' Başarıyla eklendi');
    }, error => {
      this.alertify.error(error);
      console.log(error);
    });
  }
}
