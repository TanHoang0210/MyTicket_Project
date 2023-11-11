<script>
// @ is an alias to /src
import Header from '@/components/Header.vue'
import HomeMain from '@/components/Home/HomeMain.vue'
import HomeFooter from '@/components/Home/HomeFooter.vue'
import axios from 'axios'
export default {
  name: 'HomeView',
  components: {
    Header, HomeFooter, HomeMain
  },
  data() {
    return {
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
          name: "d",
          address: "s",
          capacity: "s",
          description: "s",
          image: "",
        }
      ],
    }
  },
  beforeMount() {
    this.getVenue();
  },
  methods: {
    async getVenue() {
      const { data } = await axios.get(
        "myticket/api/venue/find",
        {
          params: {
            pageSize: 5,
            pageNumber: 1,
          },
        }
      );
      this.venues = data.data.items
    },
  }
}
</script>
<template>
  <div style="overflow: hidden;">
    <Header :isLogin="!isLogin"></Header>
    <div>
      <home-main :categories="categories" :venues="venues" :events="events">
      </home-main>
    </div>
    <div>
      <home-footer :categories="categories"></home-footer>
    </div>
  </div>
</template>
<style></style>
