<template>
	<section class="paginationContainer">
		<p>
			<b-icon icon="chevron-double-left" @click="changePage(0)"></b-icon>
			<b-icon icon="chevron-left" @click="changePage(-1)"></b-icon>
			<span class="innerPaginationContent">
				Page   {{ page }}   of   {{ pages }}
			</span>
			<b-icon icon="chevron-right" @click="changePage(1)"></b-icon>
			<b-icon icon="chevron-double-right" @click="changePage(pages)"></b-icon>
		</p>
	</section>
</template>

<script>
	export default {
		props: ['totalRecords', 'perPageOptions'],
		data: function () {
			return {
				page: 1,
				perPage: this.perPageOptions
			}
		},

		computed: {
			pages() {
				const remainder = this.totalRecords % this.perPage;
				if (remainder > 0) {
					return Math.floor(this.totalRecords / this.perPage) + 1;
				}
				else {
					return this.totalRecords / this.perPage;
				}
			}
		},

		methods: {
			changePage(val) {
				switch (val) {
					case 0: this.page = 1; break;
					case -1: this.page = this.page > 1 ? this.page - 1 : this.page; break;
					case 1: this.page = this.page < this.pages ? this.page + 1 : this.page; break;
					case this.pages: this.page = this.pages; break;
				}

				this.$emit('input', { page: this.page, perPage: this.perPage })
			}
		}
	}
</script>


<style>
	.paginationContainer {
		display: flex;
		flex-direction: row;
		color: #444;
		padding-top: 15px;
		font-size: larger;
	}

	svg {
		width: 20px;
		text-align: center;
		color: #2997FF;
		display: flex;
		align-items: center;
		margin-left: 20px;
		margin-right: 20px;
	}
</style>