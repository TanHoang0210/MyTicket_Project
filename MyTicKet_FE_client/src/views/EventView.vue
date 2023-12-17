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
              <div style="margin: 0 20px; display: flex; justify-content: space-between;">
                <h1 class="black-heading">
                  Tất Cả Sự Kiện
                </h1>
                <div style="display: flex;">
                  <ul style="display: flex; list-style: none;">
                    <li style="margin: auto; border: 1px solid var(--primary-color-bold); border-radius: 0.25rem;">
                      <b-form class="calendar-form active " @mouseleave="handleBlur"
                       @submit.prevent="searchEvent">
                      <b-input-group>
                        <b-input-group-prepend class="date-icon"> 
                          <b-icon @click="showCalendar" scale="" icon="calendar3"></b-icon>
                        </b-input-group-prepend>
                        <b-form-input @click="showCalendar" readonly style="border: none; !important"
                         :placeholder="startDate == null ? `Chọn thời gian` : `${startDate} ~ ${endDate}`"
                          type="text"></b-form-input>
                          <b-input-group-append>
                            <b-button type="submit">Tìm kiếm</b-button>
                          </b-input-group-append>
                      </b-input-group>
                        <b-row :class="{active:isShowCalendar}" class="calendar-search">
                            <b-col md="auto" style="display: flex;">
                              <b-calendar 
                              :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"
                              v-model="startDate" locale="en-US"></b-calendar>
                              <b-calendar
                              :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"
                               v-model="endDate" :min="startDate"  locale="en-US"></b-calendar>
                            </b-col>
                          </b-row>
                      </b-form>
                    </li>
                    <li style="margin: auto;">
                      <b-icon @click="renderCard()" icon="grid-fill" class="list-type"
                        :class="{ active: !isList }"></b-icon>
                    </li>
                    <li style="margin: auto;">
                      <b-icon @click="renderList()" icon="list-ul" class="list-type" :class="{ active: isList }"></b-icon>
                    </li>
                  </ul>
                </div>
              </div>
              <div>
                <div v-if="events.length === 0">
                  <div class="alert alert-notify-yellow">
                    Không có sự kiện diễn ra trong thời gian này !
                    </div>
                  </div>
                <main-event v-else :events="events" :isList="isList"></main-event>
                <footer v-if="events.length > 0" class="main__footer">
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
import MainEvent from '@/components/Home/MainComponent/EventsComponent.vue/MainEvents.vue';
import axios from 'axios'
import LoadPage from "@/views/LoadPage.vue"
export default {
  components: {
    Header, HomeFooter, MainEvent, LoadPage
  },
  data() {
    return {
      search:"",
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
      events: [{
        id: 1,
        eventName: '',
        eventTypeId: 1,
        eventTypeName: "",
        eventDescription: '',
        eventImage: '',
        firstEventDate: '',
        lastEventDate: '',
        status: 1
      }],
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
          text: 'Event',
          active: true
        },
      ]
    }
  },
  mounted() {
    this.searchEvent();
  },
  watch:{
    search: function (newSearch, oldSearch) {
      // Hành động nào đó khi biến search thay đổi
      this.searchEvent();
      // Thực hiện các hành động khác tùy thuộc vào sự thay đổi của biến
      // Ví dụ: Gọi một hàm khác, cập nhật dữ liệu khác, vv.
    }
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
    async searchEvent(){
      this.isShowCalendar = false
      await this.getAllData()
    },
    async showMore() {
      this.pageSize += 3
      const newData = await this.getEvent();
      if (newData.length > this.events.length) {
        this.events = await this.getEvent();
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
        this.events = await this.getEvent();
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
    async getEvent() {
      try {
        if(this.startDate != null && this.endDate != null){
          this.search = null
        }else{
          this.search = this.$route.query.eventName
        }
        const res = await axios.get(
          "myticket/api/event/find",
          {
            params: {
              pageSize: this.pageSize,
              pageNumber: this.pageNumber,
              startDate: this.startDate,
              endDate: this.endDate,
              keyword:this.search,
              eventTypeId:this.$route.query.eventTypeId,
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
.main-event {
  padding: 0;
  margin: 0;
}

.list-type {
  margin: auto;
  margin-left: 10px;
  margin-right: 10px;
}
.b-calendar-nav{
  background-color: #fff;
}
.list-type.active {
  color: var(--primary-color-bold);
}

.list-type:hover {
  color: var(--primary-color-bold);
  cursor: pointer;
}
.calendar-search{
  position: absolute;
  z-index: 1;
  display: flex;
  flex-direction: row;
  width: 600px;
  right: 0;
  background-color: #fff;
  display: none;
}
.calendar-search.active{
  display: block;
}
.calendar-search .col-md-auto{
  width: 250px !important;
}
.b-calendar-inner{
  width: 250px !important;
}
.list-type.active:hover {
  cursor: default;
}

.date-icon {
  margin: auto;
  margin-left: 10px;
  margin-right: 10px;
}

.date-icon:hover {
  cursor: pointer !important;
}
</style>