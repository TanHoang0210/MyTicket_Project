<script>
// @ is an alias to /src
import Header from '@/components/Header.vue'
import HomeMain from '@/components/Home/HomeMain.vue'
import HomeFooter from '@/components/Home/HomeFooter.vue'
export default {
  name: 'HomeView',
  components: {
    Header, HomeFooter, HomeMain
  },
  data() {
    return {
      isLogin:false,
      events: [{
        id: 1,
        img: 'https://i.ibb.co/qRH5qQC/slide2.jpg',
        name: 'Con Hẻm Xíu Xiu - Đêm nhạc gây quỹ cho chương trình "Như chưa hề có cuộc chia ly"',
        date: '22/10/2023',
        type: 'Theater'
      }, {
        id: 2,
        img: 'https://i.ibb.co/X5h4D5d/slide1.jpg',
        name: 'To My Soulmate | Prem Warut - 1st Date in Vietnam',
        date: '22/10/2023',
        type: 'Theater'
      }, {
        id: 3,
        img: 'https://i.ibb.co/yRx46dT/Slide3.jpg',
        name: 'Hài kịch: Lạc lối ở BangKok',
        date: '22/10/2023',
        type: 'Theater'
      }, {
        id: 4,
        img: 'https://i.ibb.co/d2hhwZX/Slide4.jpg',
        name: 'TRÁI TIM PHỤ NỮ - TRÁI TIM NGỌC TRAI - CONCERT 01 : HƯƠNG TRĂNG NHƯ NGỌC',
        date: '22/10/2023',
        type: 'Live-Music'
      }, {
        id: 5,
        img: 'https://i.ibb.co/c6TjVBs/Slide5.jpg',
        name: 'Yêu Hòa Bình 2023',
        date: '22/10/2023',
        type: 'Live-Music'
      },
      {
        id: 6,
        img: "https://i.ibb.co/Kj2yF86/sacmau.jpg",
        name: "Kịch IDECAF: Sắc Màu",
        date: '22/10/2023',
        type: 'Theater'
      }
      ],
      categories: [
        {
          id: 1,
          img: "https://www.ticketmaster.com/s3images/discovery/Concerts.jpg",
          name: "Buổi hòa nhạc",
        },
        {
          id: 2,
          img: "https://www.ticketmaster.com/s3images/discovery/Sports.jpg",
          name: "Thể thao",
        },
        {
          id: 3,
          img: "https://www.ticketmaster.com/s3images/discovery/Arts.jpg",
          name: "Sân khấu",
        },
        {
          id: 4,
          img: "https://www.ticketmaster.com/s3images/discovery/Family.jpg",
          name: "Giải trí",
        }
      ],
      venues: [
        {
          id: 1,
          img: "https://static.ticketmaster.sg/images/topic/image_50819ae078f604ae8861ef012bd371fc.jpg",
          name: "SVĐ Mỹ Đình",
        },
        {
          id: 2,
          img: "https://static.ticketmaster.sg/images/topic/image_fa45e9f53333afc475063c6b6cbb81d5.jpg",
          name: "Nhà thi đấu QK7",
        },
        {
          id: 3,
          img: "https://static.ticketmaster.sg/images/topic/image_5edb916f6355fdaf98f6031182fd532c.jpg",
          name: "SVĐ Quận 3",
        },
        {
          id: 4,
          img: "https://static.ticketmaster.sg/images/topic/image_155829ba6ef9fa9cb0472ab4f12c4ef9.jpg",
          name: "Nhà Thi Đấu 30/4",
        },
        {
          id: 5,
          img: "https://static.ticketmaster.sg/images/topic/image_9fdc9fcb3412c77fea4c2c36b10e07b9.jpg",
          name: "Nhà Thi Đấu Phú thọ",
        },
        {
          id: 6,
          img: "https://static.ticketmaster.sg/images/topic/image_9fdc9fcb3412c77fea4c2c36b10e07b9.jpg",
          name: "Sách chọn ấn",
        }
      ]
    }
  }
}
</script>
<template>
  <div style="overflow: hidden;">
    <Header
    :isLogin="!isLogin"
    ></Header>
    <div>
      <home-main
       :categories="categories"
       :venues="venues"
        :events="events"
        >
      </home-main>
    </div>
    <div>
      <home-footer :categories="categories"></home-footer>
    </div>
  </div>
</template>
<style></style>
