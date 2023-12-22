import DashboardLayout from '../layout/DashboardLayout.vue'
// GeneralViews
import NotFound from '../pages/NotFoundPage.vue'

// Admin pages
import Overview from 'src/pages/Overview.vue'
import UserProfile from 'src/pages/UserProfile.vue'
import TableList from 'src/pages/TableList.vue'
import Typography from 'src/pages/Typography.vue'
import Icons from 'src/pages/Icons.vue'
import Maps from 'src/pages/Maps.vue'
import Notifications from 'src/pages/Notifications.vue'
import Upgrade from 'src/pages/Upgrade.vue'
import Event from 'src/pages/Event/Event.vue'
import EventDetail from 'src/pages/Event/Detail/EventDetail.vue'
import CreateEvent from 'src/pages/Event/Detail/CreateEvent.vue'
import Login from 'src/pages/Login.vue'
import Customer from 'src/pages/Customer/Customer.vue'
import Supplier from 'src/pages/Supplier/Supplier.vue'
import Venue from 'src/pages/Event/Venue.vue'
import EventType from 'src/pages/Event/EventType.vue'
import Order from 'src/pages/Order/Order.vue'
import Transfer from 'src/pages/Order/Transfer.vue'
import Exchange from 'src/pages/Order/Exchange.vue'

const routes = [
  {
    name: 'Login',
    component: Login,
    path: '/login'
  },
  {
    path: '/',
    component: DashboardLayout,
    redirect: '/admin/overview',
    meta: { requiresAuth: true }
  },
  {
    path: '/admin',
    component: DashboardLayout,
    redirect: '/admin/overview',
    meta: { requiresAuth: true },
    children: [
      {
        path: 'overview',
        name: 'Overview',
        component: Overview,
    meta: { requiresAuth: true },

      },
      {
        path: 'user',
        name: 'User',
    meta: { requiresAuth: true },
    component: UserProfile,
      },
      {
        path: 'table-list',
        name: 'Table List',
        component: TableList,
    meta: { requiresAuth: true },

      },
      {
        path: 'typography',
        name: 'Typography',
        component: Typography,
    meta: { requiresAuth: true },

      },
      {
        path: 'icons',
        name: 'Icons',
        component: Icons,
    meta: { requiresAuth: true },

      },
      {
        path: 'maps',
        name: 'Maps',
        component: Maps,
    meta: { requiresAuth: true },

      },
      {
        path: 'notifications',
        name: 'Notifications',
        component: Notifications,
    meta: { requiresAuth: true },

      },
      {
        path: 'upgrade',
        name: 'Upgrade to PRO',
        component: Upgrade,
    meta: { requiresAuth: true },

      },
      {
        path: 'event',
        name: 'Event',
        component: Event,
    meta: { requiresAuth: true },

      },
      {
        path: 'venue',
        name: 'venue',
        component: Venue,
    meta: { requiresAuth: true },

      },
      {
        path: 'supplier',
        name: 'supplier',
        component: Supplier,
    meta: { requiresAuth: true },

      },
      {
        path: 'event/detail',
        name: 'EventDetail',
        component: EventDetail,
    meta: { requiresAuth: true },

      },
      {
        path: 'event/create',
        name: 'CreateEvent',
        component: CreateEvent,
    meta: { requiresAuth: true },

      },
      {
        path: 'customer',
        name: 'Customer',
        component: Customer,
    meta: { requiresAuth: true },

      },
      {
        path: 'eventtype',
        name: 'EventType',
        component: EventType,
    meta: { requiresAuth: true },

      }, {
        path: 'order',
        name: 'Order',
        component: Order,
    meta: { requiresAuth: true },

      }, {
        path: 'transfer',
        name: 'Transfer',
        component: Transfer,
    meta: { requiresAuth: true },

      },
      {
        path: 'exchange',
        name: 'Exchange',
        component: Exchange,
    meta: { requiresAuth: true },
      }
    ]
  },
  { path: '*', component: NotFound }
]
/**
 * Asynchronously load view (Webpack Lazy loading compatible)
 * The specified component must be inside the Views folder
 * @param  {string} name  the filename (basename) of the view to load.
function view(name) {
   var res= require('../components/Dashboard/Views/' + name + '.vue');
   return res;
};**/

export default routes
