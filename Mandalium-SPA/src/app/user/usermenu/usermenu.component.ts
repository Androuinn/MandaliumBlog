import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { BlogService } from 'src/app/_services/blog.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { Router } from '@angular/router';
import { PhotoService } from 'src/app/_services/photo.service';
import { Photo } from 'src/app/_models/Photo';
import { Writer } from 'src/app/_models/Writer';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-usermenu',
  templateUrl: './usermenu.component.html',
  styleUrls: ['./usermenu.component.css'],
})
export class UsermenuComponent implements OnInit {
  blogEntries: BlogEntry[];
  pagination: Pagination;
  postsOpen = false;
  writer: Writer = {
    id: null,
    surname: '',
    name: '',
    photoUrl: '',
    birthDate: null,
    background: '',
  };
  writerForm: FormGroup;
  writerUpdateOpen = false;
  writerProfilePhotoUpdate = false;
  file: File;

  constructor(
    private router: Router,
    public authService: AuthService,
    private blogService: BlogService,
    private userService: UserService,
    private alertify: AlertifyService,
    private formBuilder: FormBuilder,
    private photoService: PhotoService
  ) {
    this.writerForm = this.formBuilder.group({
      id: Number,
      name: ['', [Validators.required, Validators.maxLength(100)]],
      surname: ['', [Validators.required, Validators.maxLength(100)]],
      birthdate: [Date],
      background: [''],
      photoUrl: [''],
    });
  }

  ngOnInit() {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 15,
      totalPages: 1,
      totalItems: 1,
    };
    this.getWriter();
  }

  openPosts() {
    this.loadBlogEntries();
    this.postsOpen = !this.postsOpen;
  }

  openUpdateProfile() {
    this.writerUpdateOpen = !this.writerUpdateOpen;
    this.writerForm.setValue({
      name: this.writer.name,
      surname: this.writer.surname,
      id: this.writer.id,
      background: this.writer.background,
      birthdate: this.writer.birthDate,
      photoUrl: this.writer.photoUrl,
    });
  }

  loadBlogEntries() {
    this.blogService
      .getBlogEntries(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        null,
        this.authService.decodedToken.nameid
      )
      .subscribe(
        (res: PaginatedResult<BlogEntry[]>) => {
          this.blogEntries = res.result;
          this.pagination = res.pagination;
        },
        (error) => {
          console.error(error);
        }
      );
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadBlogEntries();
  }

  // post numarasını sıfıra çekiyor eğer değiştirme yaptıysa 1 de kalıyor
  changeEntry(entry: number) {
    this.blogService.changeBlogEntry(entry);
    this.router.navigate(['/create']);
  }

  getWriter() {
    this.userService.getWriter().subscribe((res: Writer) => {
      this.writer = res;
    });
  }

  updateWriter() {
    this.userService.updateWriter(this.writerForm.value).subscribe(
      () => {
        this.alertify.success('Güncelleme Başarılı');
        this.getWriter();
        this.writerUpdateOpen = !this.writerUpdateOpen;
      },
      (error) => {
        this.alertify.error('Güncelleme Başarısız');
      }
    );
  }

  onFileChanged(event: any) {
    this.file = event.target.files[0];
  }

  deleteBlogEntry(id: number) {
    console.log(id);
    return this.blogService.deleteBlogEntry(id).subscribe(
      () => {
        this.loadBlogEntries();
        this.alertify.success('Silme işlemi Başarılı');
      },
      (error) => {
        this.alertify.error('Silme işlemi Başarısız');
        console.log(error);
      }
    );
  }

  updateProfilePhoto() {
    const formData: FormData = new FormData();
    formData.append('file', this.file, this.file.name);
    formData.append('writerId', this.authService.decodedToken.nameid);
    console.log(formData);
    if (this.file != null) {
      return this.photoService.updateProfilePhoto(formData).subscribe(
        (res) => {
          this.alertify.success('fotoğraf yükleme başarılı');
          this.writer.photoUrl = res.toString();
          this.router.navigate(['/profile']);
          this.file = null;
          this.writerProfilePhotoUpdate = !this.writerProfilePhotoUpdate;
        },
        (error) => {
          console.error(error);
          this.alertify.error(error);
        }
      );
    }
  }
}
