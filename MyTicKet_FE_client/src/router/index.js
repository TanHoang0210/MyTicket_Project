import Vue from 'vue'
import VueRouter from 'vue-router'
import HomeView from '../views/HomeView.vue'
import EventDetailView from '../views/EventDetailView.vue'
import PageNotFound from '../views/NotFound.vue'
import BuyTicket from '../views/BuyTicketView.vue'
import store from '../store';
Vue.use(VueRouter)

const routes = [
  { path: '*', component: PageNotFound },
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/login',
    name: 'login',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/LoginView.vue')
  },
  {
    path: '/signup',
    name: 'signup',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/SignUpView.vue')
  },
  {
    path: '/event/detail',
    name: 'eventDetail',
    component: EventDetailView
  },
  {
    path: '/event',
    name: 'event',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/EventView.vue')
  },
  {
    path: '/ticket/:type/:id',
    name: 'buyTicket',
    component:BuyTicket
  },
  {
    path: '/ticket',
    name: 'Ticket',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/BuyTicketView.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    } else if (to.hash) {
      return {
        selector: to.hash
      };
    } else {
      return { x: 0, y: 0 };
    }
  },
})
// // router.beforeEach((to, from, next) => {
// //   Kiểm tra xem người dùng đã đăng xuất chưa
// //   const isLoggedOut = store.getters.currentUser === null;

// //   if (to.matched.some(route => route.meta.requiresAuth) && isLoggedOut) {
// //     Nếu route yêu cầu xác thực và người dùng đã đăng xuất, chuyển hướng đến trang chủ
// //     next('/');
// //   } else {
// //     Nếu người dùng đã đăng nhập hoặc route không yêu cầu xác thực, cho phép điều hướng
// //     next();
// //   }
// // });

export default router
