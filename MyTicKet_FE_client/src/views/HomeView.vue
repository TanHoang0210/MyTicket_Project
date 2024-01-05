<script>
// @ is an alias to /src
import Header from '@/components/Header.vue'
import HomeMain from '@/components/Home/HomeMain.vue'
import HomeFooter from '@/components/Home/HomeFooter.vue'
import axios from 'axios'
import LoadPage from "@/views/LoadPage.vue"
export default {
  name: 'HomeView',
  components: {
    Header, HomeFooter, HomeMain, LoadPage
  },
  data() {
    return {
      isLoading: false,
      isSuccess: false,
      isError: false,
      events: [{
        id: 0,
        eventName: "string",
        eventTypeId: 0,
        eventTypeName: "string",
        eventDescription: "string",
        eventImage: "string",
        admissionPolicy: "string",
        exchangePolicy: "string",
        status: 0,
        firstEventDate: "2023-12-16T09:07:24.982Z",
        lastEventDate: "2023-12-16T09:07:24.982Z",
        supplierId: 0,
        supllier: "string",
        address: "string",
      }],
      eventTopSales: [{
        id: 0,
        eventName: "string",
        eventTypeId: 0,
        eventTypeName: "string",
        eventDescription: "string",
        eventImage: "string",
        admissionPolicy: "string",
        exchangePolicy: "string",
        status: 0,
        firstEventDate: "2023-12-16T09:07:24.982Z",
        lastEventDate: "2023-12-16T09:07:24.982Z",
        supplierId: 0,
        supllier: "string",
        address: "string",
      }],
      eventNews: [{
        id: 0,
        eventName: "string",
        eventTypeId: 0,
        eventTypeName: "string",
        eventDescription: "string",
        eventImage: "string",
        admissionPolicy: "string",
        exchangePolicy: "string",
        status: 0,
        firstEventDate: "2023-12-16T09:07:24.982Z",
        lastEventDate: "2023-12-16T09:07:24.982Z",
        supplierId: 0,
        supllier: "string",
        address: "string",
      }],
      categories: [
        {
          id: 1,
          name: "",
          description: "",
          eventTypeImage: "",
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
  mounted() {
    this.getAllData();
    console.log("Đường dẫn hiện tại:", this.$route.query);
  },
  methods: {
    async getAllData() {
      this.isLoading = true;
      try {
        // Gọi nhiều API sử dụng async/await
        this.eventNews = await this.getEventNew();
        this.venues = await this.getVenue();
        this.events = await this.getEvent();
        this.categories = await this.getCategories();
        this.eventTopSales = await  this.getEventTopSale()
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
              pageSize: 6,
              pageNumber: 1,
            },
          }
        )
        return res.data.data.items;
      } catch (error) {
        console.error('API 1 Error:', error);
        throw error;
      }
    },
    async getEvent() {
      try {
        const res = await axios.get(
          "myticket/api/event/find-top-standing",
          {
            params: {
              pageSize: 9,
              pageNumber: 1,
            },
          }
        )
        return res.data.data;
      } catch (error) {
        console.error('API 1 Error:', error);
        throw error;
      }
    },
    async getEventNew() {
      try {
        const res = await axios.get(
          "myticket/api/event/find-top-new"
        )
        return res.data.data;
      } catch (error) {
        console.error('API 1 Error:', error);
        throw error;
      }
    },
    async getEventTopSale() {
      try {
        const res = await axios.get(
          "myticket/api/event/find-top-sale"
        )
        return res.data.data;
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
              pageNumber: 1,
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
<template>
  <div>
    <div v-if="isLoading">
      <LoadPage />
    </div>
    <div v-if="isSuccess" style="overflow: hidden;">
      <Header :isLogin="!isLogin"></Header>
      <div>
        <home-main :categories="categories" :venues="venues" :events="events" :eventNews="eventNews" :eventTopSales="eventTopSales">
        </home-main>
      </div>
      <div>
        <home-footer :categories="categories"></home-footer>
      </div>
    </div>
    <div style="display: flex; flex-direction: column;" v-if="isError">
      <Header :isLogin="!isLogin"></Header>
      <div>
        <home-main :categories="categories" :venues="venues" :events="events">
        </home-main>
      </div>
      <div>
        <home-footer :categories="categories"></home-footer>
      </div>
    </div>
  </div>
</template>
<style></style>
