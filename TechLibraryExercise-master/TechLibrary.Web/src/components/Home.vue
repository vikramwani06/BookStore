<template>
	<div class="home">
		<nav />
		<main>
			<b-container>
				<b-row>
					<b-col sm="12">
						<b-form-input v-model="filter" type="search" placeholder="Search"></b-form-input>
					</b-col>
				</b-row>

			</b-container>

			<b-container class="pagination">
				<b-row>
					<b-col sm="12">
						<Pagination v-if="dataContext" :totalRecords="responseLength" :perPageOptions="10" v-model="pagination" />
					</b-col>
				</b-row>


				<b-row>
					<b-col sm="12">
						<b-button v-b-modal.modal-prevent-closing variant="primary">Add Book</b-button>
					</b-col>
				</b-row>
			</b-container>
			<!--<b-container class="pagination">
				<b-row>
					<b-col sm="9">
						<b-pagination v-if="dataContext" size="md" :total-rows="responseLength" v-model="pagination" :per-page="10">
						</b-pagination>
					</b-col>
				</b-row>
			</b-container>-->

			<h1>{{ msg }}</h1>

			<b-table striped hover :items="computedtableData" :filter="filter" :fields="fields" responsive="sm" current-page="currentPage" per-page="0">
				<template v-slot:cell(thumbnailUrl)="data">
					<b-img :src="data.value" thumbnail fluid></b-img>
				</template>
				<template v-slot:cell(title_link)="data">
					<b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
				</template>
			</b-table>

			<!--Modal Starts Here-->
			<b-modal id="modal-prevent-closing" ref="modal" title="Add a Book" @show="resetModal" @hidden="resetModal" @ok="handleOk">
				<form ref="form" @submit.stop.prevent="handleSubmit">

					<b-form-group label="Title" label-for="title-input" invalid-feedback="Title is required" :state="titleState">
						<b-form-input id="title-input" v-model="title" :state="titleState" required></b-form-input>
					</b-form-group>
					<b-form-group label="ISBN" label-for="isbn-input" invalid-feedback="Isbn is required" :state="isbnState">
						<b-form-input id="isbn-input" v-model="isbn" :state="isbnState" required></b-form-input>
					</b-form-group>
					<b-form-group label="PublishedDate" label-for="date-input" invalid-feedback="Published Date is required" :state="dateState">
						<b-form-input id="date-input" v-model="publishedDate" :state="dateState" required></b-form-input>
					</b-form-group>
					<b-form-group label="ThumbnailURL" label-for="image-input" :state="imageState">
						<b-form-input id="image-input" v-model="thumbnailUrl" :state="imageState"></b-form-input>
					</b-form-group>
					<b-form-group label="Description" label-for="desc-input" invalid-feedback="Description is required" :state="descState">
						<b-form-input id="desc-input" v-model="descr" :state="descState" required></b-form-input>
					</b-form-group>
				</form>
			</b-modal>
			<!--Modal Ends Here-->

		</main>
	</div>
</template>

<script>
	import Pagination from '../components/Pagination'
	import axios from 'axios';

	export default {

		components: {
			Pagination
		},
		name: 'Home',
		props: {
			msg: String,
		},
		data: function () {
			return {
				perPage: 10,
				currentPage: 1,
				filter: "",
				responseLength: "",
				tableData: [],
				pagination: { page: 1, perPage: 10 },

				title: '',
				isbn: '',
				publishedDate: '',
				thumbnailUrl: '',
				descr: '',
				titleState: null,
				isbnState: null,
				dateState: null,
				imageState: null,
				descState: null,

				fields: [
					{ key: 'thumbnailUrl', label: 'Book Image' },
					{ key: 'title_link', label: 'Book Title', sortable: true, sortDirection: 'desc' },
					{ key: 'isbn', label: 'ISBN', sortable: true, sortDirection: 'desc' },
					{ key: 'descr', label: 'Description', sortable: true, sortDirection: 'desc' }
				],
				items: []
			}
		},
		computed: {
			computedtableData() {
				if (!this.tableData) return [];

				else {
					const firstIndex = (this.pagination.page - 1) * this.pagination.perPage;
					const lastIndex = (this.pagination.page) * this.pagination.perPage;
					return this.tableData.slice(firstIndex, lastIndex);
				}
			},

			filteredList: function () {
				return this.tableData.filter((book) => {
					return (book.title.match(this.filter) || book.desc.match(this.filter));
				});
			},

			rows() {
				return this.responseLength;
			}
		},
		created() {
			fetch('https://localhost:5001/books')
				.then(response => response.json())
				.then(json => {
					this.tableData = json
					this.responseLength = this.tableData.length;
				})
		},

		methods: {
			dataContext(ctx, callback) {
				axios.get("https://localhost:5001/books")
					.then(response => {
						callback(response.data);
					});
			},

			addBook() {
				axios.post('https://localhost:5001/books', {
					title: this.title,
					isbn: this.isbn,
					publisheddate: this.publishedDate,
					thumbnailUrl: this.thumbnailUrl,
					descr: this.descr
				})
					.then(function (response) {
						alert('Book Added');
						console.log(response);
					})

			},

			checkFormValidity() {
				const valid = this.$refs.form.checkValidity();
				this.titleState = valid;
				this.isbnState = valid;
				this.dateState = valid;
				this.imageState = valid;
				this.descState = valid;
				return valid;
			},
			resetModal() {
				this.title = '';
				this.isbn = '';
				this.publishedDate = '';
				this.description = '';
				this.titleState = null
				this.isbnState = null;
				this.dateState = null;
				this.imageState = null;
				this.descState = null;
			},

			handleOk(bvModalEvt) {
				// Prevent modal from closing
				bvModalEvt.preventDefault()
				// Trigger submit handler
				this.handleSubmit()
			},

			handleSubmit() {
				// Exit when the form isn't valid
				if (!this.checkFormValidity()) {
					return;
				}
				this.addBook();
				

				// Hide the modal manually
				this.$nextTick(() => {
					this.$bvModal.hide('modal-prevent-closing')
				})
			}
		}
	};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
	main {
		margin: 20px;
	}

	.pagination {
		display: flex;
		flex-direction: row;
		justify-content: space-between;
		margin: 1em 0em;
		align-items: center;
	}

	.filter {
		margin-top: 2em;
	}
</style>

