<template>
  <div class="wrapper">
    <side-bar>
      <mobile-menu slot="content"></mobile-menu>
      <sidebar-link to="/admin/overview">
        <i class="nc-icon nc-chart-pie-35"></i>
        <p>Tổng quan</p>
      </sidebar-link>
      <sidebar-link to="/admin/customer">
        <i class="nc-icon nc-single-02"></i>
        <p>Khách hàng</p>
      </sidebar-link>
      <sidebar-link to="/admin/supplier">
        <i class="nc-icon nc-circle-09"></i>
        <p>Nhà cung cấp</p>
      </sidebar-link>
      <sidebar-link to="/admin/eventtype">
        <i class="nc-icon nc-support-17"></i>
        <p>Loại sự kiện</p>
      </sidebar-link>
      <sidebar-link to="/admin/event">
        <i class="nc-icon nc-notes"></i>
        <p>Sự kiện</p>
      </sidebar-link>
      <sidebar-link to="/admin/venue">
        <i class="nc-icon nc-pin-3"></i>
        <p>Sân vận động</p>
      </sidebar-link>
      <!-- <sidebar-link to="/admin/order">
        <i class="nc-icon nc-atom"></i>
        <p>Đơn đặt vé</p>
      </sidebar-link>
      <sidebar-link to="/admin/transfer">
        <i class="nc-icon nc-refresh-02"></i>
        <p>Đơn chuyển nhượng</p>
      </sidebar-link>
      <sidebar-link to="/admin/exchange">
        <i class="nc-icon nc-send"></i>
        <p>Yêu cầu trả vé</p>
      </sidebar-link> -->
      <!-- <sidebar-link to="/admin/icons">
        <i class="nc-icon nc-atom"></i>
        <p>Quản lý vé</p>
      </sidebar-link> -->
      <sidebar-link to="/admin/report">
        <i class="nc-icon nc-bell-55"></i>
        <p>Thông báo</p>
      </sidebar-link>
    </side-bar>
    <div class="main-panel">
      <top-navbar></top-navbar>

      <dashboard-content @click="toggleSidebar">

      </dashboard-content>

      <content-footer></content-footer>
    </div>
  </div>
</template>
<style lang="scss">
.sidebar-wrapper{
  background-color: var(--primary-color-bold) !important;
}
</style>
<script>
  import TopNavbar from './TopNavbar.vue'
  import ContentFooter from './ContentFooter.vue'
  import DashboardContent from './Content.vue'
  import MobileMenu from './MobileMenu.vue'
  export default {
    components: {
      TopNavbar,
      ContentFooter,
      DashboardContent,
      MobileMenu
    },
    methods: {
      toggleSidebar () {
        if (this.$sidebar.showSidebar) {
          this.$sidebar.displaySidebar(false)
        }
      }
    }
  }

</script>
