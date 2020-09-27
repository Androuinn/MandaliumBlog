import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { BlogService } from 'src/app/_services/blog.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { Router, ActivatedRoute } from '@angular/router';
import { PhotoService } from 'src/app/_services/photo.service';
import { User } from 'src/app/_models/Writer';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { MetaService } from 'src/app/_services/meta.service';

@Component({
  selector: 'app-usermenu',
  templateUrl: './usermenu.component.html',
  styleUrls: ['./usermenu.component.css'],
})
export class UsermenuComponent implements OnInit, AfterViewChecked {
  blogEntries: BlogEntry[];
  pagination: Pagination;
  postsOpen = false;
  user: User = {
    id: null,
    surname: '',
    name: '',
    photoUrl: '',
    birthDate: null,
    background: '',
  };
  userForm: FormGroup;
  userUpdateOpen = false;
  userProfilePhotoUpdate = false;
  file: File;
  public Editor = ClassicEditor;
  fragment: any;


  constructor(
    private router: Router,
    public authService: AuthService,
    private blogService: BlogService,
    private userService: UserService,
    private alertify: AlertifyService,
    private formBuilder: FormBuilder,
    private photoService: PhotoService,
    private route: ActivatedRoute,
    private metaService: MetaService
  ) {
    this.userForm = this.formBuilder.group({
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
    this.getUser();

    this.metaService.UpdateTags('Hesap', 'Kullanıcı hesap bilgileri', null, null, null, null);
  }

  ngAfterViewChecked(): void {
    try {
        if (this.fragment) {
            document.querySelector('#' + this.fragment).scrollIntoView();
        }
    } catch (e) { }
  }

  openPosts() {
    this.loadBlogEntries();
    this.postsOpen = !this.postsOpen;
  }

  openUpdateProfile() {
    this.route.fragment.subscribe(fragment => { this.fragment = fragment; });
    this.userUpdateOpen = !this.userUpdateOpen;
    this.userForm.setValue({
      name: this.user.name,
      surname: this.user.surname,
      id: this.user.id,
      background: this.user.background,
      birthdate: this.user.birthDate,
      photoUrl: this.user.photoUrl,
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
    this.blogService.changeBlogEntryForCreationOrUpdate(entry);
    this.router.navigate(['/create']);
  }

  getUser() {
    this.userService.getUser().subscribe((res: User) => {
      this.user = res;
      if (this.user.photoUrl == null || this.user.photoUrl === '') {
        this.user.photoUrl = '../../assets/çzgisiz logo.png';
      }
    });
  }

  updateUser() {
    this.userService.updateUser(this.userForm.value).subscribe(
      () => {
        this.alertify.success('Güncelleme Başarılı');
        this.getUser();
        this.userUpdateOpen = !this.userUpdateOpen;
      },
      (error) => {
        this.alertify.error('Güncelleme Başarısız');
        console.log(error);
      }
    );
  }

  onFileChanged(event: any) {
    this.file = event.target.files[0];
  }

  deleteBlogEntry(id: number) {
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
    formData.append('userId', this.authService.decodedToken.nameid);
    console.log(formData);
    if (this.file != null) {
      return this.photoService.updateProfilePhoto(formData).subscribe(
        (res) => {
          this.alertify.success('fotoğraf yükleme başarılı');
          this.user.photoUrl = res.toString();
          this.router.navigate(['/profile']);
          this.file = null;
          this.userProfilePhotoUpdate = !this.userProfilePhotoUpdate;
        },
        (error) => {
          console.error(error);
          this.alertify.error(error);
        }
      );
    }
  }
}
