<template>
  <div class="content">
    <div class="container-fluid">
      <div class="row">
        <div class="col-12">
          <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
            <template slot="header">
              <div class="row" style="display: flex; justify-content: space-between;">
                <div class="col-md-3">
                  <h4 class="card-title">Danh sách sự kiện</h4>
                </div>
                <div class="col-md-5">
                  <b-input-group class="float-right">
                    <b-form-input v-model="search.keyword"></b-form-input>
                    <b-input-group-append>
                      <b-button @click="getAllData()" variant="outline-success">Tìm kiếm</b-button>
                    </b-input-group-append>
                  </b-input-group>
                </div>
                <div class="col-md-2">
                  <b-form-select v-model="search.status" @change="getAllData()">
                    <b-form-select-option :value="null">Tất cả</b-form-select-option>
                    <b-form-select-option v-for="item in eventStatuses" :value="item.id">{{ item.name }}</b-form-select-option>
                  </b-form-select>
                </div>
                <div class="col-md-2">
                  <router-link class="btn btn-success btn-fill float-right" style="color: #fff; margin-right: 30px;"
                    :to="{ name: 'CreateEvent' }">
                    Thêm mới
                  </router-link>
                </div>
              </div>
            </template>
            <l-table class="table-hover table-striped" :columns="table1.columns" :data="formattedData"
              :action="table1.action">
              <template v-slot:cell(status)="props">
                <span :style="{ color: props.row.statusColor }">{{ props.row.status }}</span>
              </template>
            </l-table>
          </card>
        </div>
      </div>
    </div>
  </div>
</template>
  
<script>
import LTable from 'src/components/Table.vue'
import Card from 'src/components/Cards/Card.vue'
import axios from 'axios'

const tableColumns = [
  { label: "ID", field: "id" },
  { label: "Tên sự kiện", field: "eventName" },
  { label: "Loại sự kiện", field: "eventTypeName" },
  { label: "Nhà cung cấp", field: "supllier" },
  { label: "Trạng thái", field: "status" }
];

export default {
  components: {
    LTable,
    Card
  },
  data() {
    return {
      search:{
        keyword:null,
        status:null
      },
      eventStatuses:[
          {
            id:1,
            name:"Khởi tạo"
          },
          {
            id:2,
            name:"Mở bán vé"
          },
          {
            id:3,
            name:"Ngừng bán vé"
          },
          {
            id:4,
            name:"Đang diễn ra"
          },
          {
            id:5,
            name:"Đã hủy"
          },
      ],
      id: 1,
      pageSize: 100,
      pageNumber: 1,
      table1: {
        columns: tableColumns,
        data: [],
        action: 'event/detail'
      },
    };
  },
  methods: {
    async getEvent() {
      try {
        const res = await axios.get(
          "myticket/api/event/find",
          {
            params: {
              pageSize: this.pageSize,
              pageNumber: this.pageNumber,
              startDate: this.startDate,
              endDate: this.endDate,
              keyword: this.search.keyword,
              eventTypeId: this.$route.query.eventTypeId,
              status:this.search.status
            },
          }
        )
        return res.data.data.items;
      } catch (error) {
        console.error('API Error:', error);
        throw error;
      }
    },
    async getAllData() {
      try {
        this.table1.data = await this.getEvent();
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    },
    formatStatus(status) {
      let statusColor = "blue";

      switch (status) {
        case 1:
          statusColor = "green"; // Đổi màu thành xanh lá cây cho status 1
          return { status: "Khởi tạo", statusColor };
        case 2:
          statusColor = "orange"; // Đổi màu thành cam cho status 2
          return { status: "Đang mở bán", statusColor };
        case 3:
          statusColor = "red"; // Đổi màu thành đỏ cho status 3
          return { status: "Ngừng bán vé", statusColor };
        case 4:
          statusColor = "purple"; // Đổi màu thành tím cho status 4
          return { status: "Đang diễn ra", statusColor };
        case 5:
          statusColor = "gray"; // Đổi màu thành xám cho status 5
          return { status: "Đã hủy", statusColor };
        default:
          return { status, statusColor };
      }
    }
  },
  mounted() {
    this.getAllData();
  },
  computed: {
    formattedData() {
      return this.table1.data.map(element => ({
        ...element,
        ...this.formatStatus(element.status)
      }));
    }
  }
};
</script>
  
<style></style>
  