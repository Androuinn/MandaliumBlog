import { Injectable } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class MetaService {
  date = new Date();

constructor(private metaTagService: Meta, private titleService: Title) { }




CreateSiteTags() {
  this.metaTagService.addTags([
    { name: 'decpription', content: 'Mandalium | Blog Dünyasının Vazgeçilmez Adresi, Eğlence ve Haber Sitesi'},
    { name: 'keywords', content: 'Mandalium, Blog Dünyasının Vazgeçilmez Adresi, Eğlence ve Haber Sitesi, '},
    { name: 'robots', content: 'index, follow'},
    { name: 'author', content: 'Tugay Mandal'},
    { name: 'date', content:  this.date.toString() , scheme: 'DD-MM-YYYY'},
    { property: 'og:site_name', content: 'Mandalium'},
    { property: 'og:url', content: 'https://mandalium.azurewebsites.net'},
    { property: 'og:image', content: 'https://res.cloudinary.com/dpwbfco4g/image/upload/v1587061001/%C3%A7zgisiz_logo_ddiqau.png'},
    { property: 'og:title', content: 'Mandalium | En son Haberler'},
    { property: 'og:description', content: 'Mandalium '}
  ]);
}


UpdateTags(title: string, description: string, ogUrl: string, ogTitle: string, ogDescription: string, ogImage: string ) {
  this.metaTagService.updateTag({
    name: 'description',
    content: description,
  });
  this.metaTagService.updateTag({property: 'og:site_name', content: 'Mandalium'});
  this.metaTagService.updateTag({property: 'og:url', content: 'https://mandalium.azurewebsites.net/' +  ogUrl});
  this.metaTagService.updateTag({property: 'og:title', content: ogTitle});
  this.metaTagService.updateTag({property: 'og:description', content: ogDescription});

  if (ogImage != null || ogImage !== '') {
    this.metaTagService.updateTag({
      property: 'og:image',
      content: ogImage,
    });
    this.titleService.setTitle(title);
  } else {
    this.metaTagService.updateTag({
      property: 'og:image',
      content: 'https://mandalium.azurewebsites.net/assets/çzgisiz logo.png',
    });
    this.titleService.setTitle(title);
  }
}

}
