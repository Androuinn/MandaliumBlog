import { Component, OnInit } from '@angular/core';
import { Writer } from 'src/app/_models/Writer';
import { Topic } from 'src/app/_models/Topic';
import { BlogService } from 'src/app/_services/blog.service';
import { WriterTopic } from 'src/app/_models/WriterTopic';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-blog-entry',
  templateUrl: './create-blog-entry.component.html',
  styleUrls: ['./create-blog-entry.component.css']
})
export class CreateBlogEntryComponent implements OnInit {
  writers: Writer[];
  topics: Topic[];
  innerTextHtml: string;
  createBlogPost: FormGroup;

  constructor(private blogService: BlogService, formBuilder: FormBuilder, private router: Router) {
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
      this.blogService.saveBlogEntry(this.createBlogPost.value).subscribe(() => {
        console.log('başarılı');
        this.router.navigate(['/']);
      }, error => {
        console.error(error);
      });

    }
   }

   addContent(content: string) {
     document.getElementById('textarea').innerText += content;
     this.innerTextHtml += content;
   }

  getInnerHtml(newValue: string) {
    this.innerTextHtml = newValue;
  }
}
