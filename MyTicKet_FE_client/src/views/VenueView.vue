<template>
  <div>
    <div v-if="isLoading">
      <LoadPage />
    </div>
    <div class="main-event">
      <div>
        <Header :isLogin="!isLogin"></Header>
        <div style="display: block; height: 50px; background-color: var(--primary-color-bold); margin-bottom:20px ;">
          <b-breadcrumb class="event-breadcrumb black-breadcrumb" :items="breadItems"></b-breadcrumb>
        </div>
      </div>
      <main class="main">
        <div class="main__container">
          <div class="home__container">
            <div class="main__container--inner">
                <div v-if="venues.length === 0">
                  <div class="alert alert-notify-yellow">
                    Không có sự kiện diễn ra trong thời gian này !
                    </div>
                  </div>
                <venues v-else :venues="venues" :isList="isList"></venues>
                <footer v-if="venues.length > 0" class="main__footer">
                  <div class=" more--btn main__footer--inner">
                    <b-button v-if="!noMoreData" @click="showMore()" class="main__footer--btn">
                      Xem Thêm
                    </b-button>
                    <b-button v-else @click="showLess()" class="main__footer--btn">
                      Ít Hơn
                    </b-button>
                  </div>
                </footer>
            </div>
          </div>
        </div>
      </main>
      <div>
        <home-footer :categories="categories"></home-footer>
      </div>
    </div>
  </div>
</template>
<script>
import Header from '@/components/Header.vue'
import HomeFooter from '@/components/Home/HomeFooter.vue'
import Venues from '@/components/Home/MainComponent/Venues.vue';
import axios from 'axios'
import LoadPage from "@/views/LoadPage.vue"
export default {
  components: {
    Header, HomeFooter, Venues, LoadPage
  },
  data() {
    return {
      isShowCalendar:false,
      noMoreData: false,
      pageSize: 9,
      pageNumber: 1,
      isList: false,
      isLoading: false,
      isSuccess: false,
      isError: false,
      isList: false,
      startDate: null,
      endDate: null,
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
      categories: [
        {
          id: 1,
          name: "",
          description: "",
          eventTypeImage: "",
        }
      ],
      breadItems: [
        {
          text: 'Home',
          href: '/'
        },
        {
          text: 'Venue',
          active: true
        },
      ]
    }
  },
  mounted() {
    this.getAllData();
  },
  methods: {
    renderCard() {
      if (this.isList)
        this.isList = false;
      this.getAllData()
    },
    renderList() {
      if (!this.isList)
        this.isList = true;
      this.getAllData()
    },
    showCalendar(){
      this.isShowCalendar = !this.isShowCalendar;
    },
    handleBlur(){
    this.isShowCalendar = false;   
    },
    async searchEvent(event){
      event.preventDefault()
      this.isShowCalendar = false
      await this.getAllData()
    },
    async showMore() {
      this.pageSize += 3
      const newData = await this.getVenue();
      if (newData.length > this.venues.length) {
        this.venues = await this.getVenue();
        await this.getAllData()
      }
      else {
        this.noMoreData = true;
        this.$toasted.error("Không còn thêm dữ liệu để hiển thị", {
          position: 'top-right',
          duration: 3000, // Thời gian hiển thị toast (ms)
          theme: 'outline', // Theme: 'outline', 'bubble'
          iconPack: 'fontawesome', // Icon pack: 'fontawesome', 'mdi'
          icon: 'time', // Tên icon, ví dụ: 'check' (cho fontawesome)
          iconColor: 'white', // Màu của icon
          containerClass: 'custom-toast-container-class', // Thêm class cho container
          singleton: true, // Hiển thị toast duy nhất, không hiển thị toast mới nếu toast trước chưa biến mất
        });
      }
    },
    async showLess() {
      this.pageSize = 3
      this.events = await this.getEvent();
      this.noMoreData = false;
      await this.getAllData()
    },
    async getAllData() {
      this.isLoading = true;
      try {
        this.venues = await this.getVenue();
        this.categories = await this.getCategories();
        this.isSuccess = true;
        this.isLoading = false;
      } catch (error) {
        this.isError = true;
        this.isLoading = false;
        this.$toasted.error('Oops! Đã xảy ra lỗi! Vui lòng thử lại', {
          position: 'top-right',
          duration: 3000, // Thời gian hiển thị toast (ms)
        });
      }
    },
    async getVenue() {
      try {
        const res = await axios.get(
          "myticket/api/venue/find",
          {
            params: {
              pageSize: this.pageSize,
              pageNumber: this.pageNumber,
              keyword:this.$route.query.eventName,
            },
          }
        )
        return res.data.data.items;
      } catch (error) {
        console.error('API 1 Error:', error);
        throw error;
      }
    },
    async getCategories() {
      try {
        const res = await axios.get(
          "myticket/api/event-type/find",
          {
            params: {
              pageSize: 4,
              pageNumber: 1
            },
          }
        )
        return res.data.data.items;
      } catch (error) {
        console.error('API 1 Error:', error);
        throw error;
      }
    }
  }
}
</script>
<style>

</style>