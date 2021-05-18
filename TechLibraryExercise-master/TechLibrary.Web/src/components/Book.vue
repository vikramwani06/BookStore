<template>
	<div v-if="book">

		<b-card :title="book.title"
				:img-src="book.thumbnailUrl"
				img-alt="Image"
				img-top
				tag="article"
				style="max-width: 30rem;"
				class="mb-2">
			<b-card-text>
				{{ book.descr }}

			</b-card-text>

			<b-button to="/" variant="primary">Back</b-button>
			<b-button style="margin-left:10px;" v-b-modal.modal-prevent-closing variant="primary">Edit Book Details</b-button>
		</b-card>

		<b-modal id="modal-prevent-closing" ref="modal" title="Edit Book Details" @show="resetModal" @hidden="resetModal" @ok="handleOk">
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
	</div>
	<!--Modal starts Here-->
	<!--Modal Ends Here-->

</template>

<script>

	import axios from 'axios';

	export default {
		name: 'Book',
		props: ["id"],
		data() {
			return {
				book: null,

				/*Modal Variables*/
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
				updateParams: [],
			}
		},

		methods: {
			updateBook() {

				return axios.put('https://localhost:5001/books/' + this.id, {
					/*id: this.id,*/
					title: this.title,
					isbn: this.isbn,
					publisheddate: this.publishedDate,
					thumbnailUrl: this.thumbnailUrl,
					descr: this.descr
				})
					.then(function (response) {
						alert('Book Updated');
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
				this.updateParams.push(this.title, this.description);
				this.updateBook(this.updateParams);
				console.log('Params', this.updateParams);
				// Hide the modal manually
				this.$nextTick(() => {
					this.$bvModal.hide('modal-prevent-closing')
				})
			}
		},

		mounted() {
			axios.get(`https://localhost:5001/books/${this.id}`)
				.then(response => {
					this.book = response.data;
				});
		}
	}
</script>
