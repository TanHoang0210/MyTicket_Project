<template>
  <div class="content">
    <div class="container-fluid">
      <div class="row">
        <div class="col-12">
          <card class="strpied-tabled-with-hover" body-classes="table-full-width table-responsive">
            <template slot="header">
              <div style="display: flex; justify-content: space-between;">
                <h4 class="card-title">Danh sách sự kiện</h4>
                <router-link class="btn btn-success btn-fill" style= "color: #fff; margin-right: 30px;" :to="{ name: 'CreateEvent' }">
                 Thêm mới
                </router-link>
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
      id: 1,
      pageSize: 10,
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
              keyword: this.$route.query.eventName,
              eventTypeId: this.$route.query.eventTypeId
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
  