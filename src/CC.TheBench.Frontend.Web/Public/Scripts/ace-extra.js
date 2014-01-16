
	sidebar_collapsed : function(collpase) {
		collpase = collpase || false;

		var sidebar = document.getElementById('sidebar');
		var icon = document.getElementById('sidebar-collapse').querySelector('[class*="icon-"]');
		var $icon1 = icon.getAttribute('data-icon1');//the icon for expanded state
		var $icon2 = icon.getAttribute('data-icon2');//the icon for collapsed state

		if(collpase) {
			ace.addClass(sidebar , 'menu-min');
			ace.removeClass(icon , $icon1);
			ace.addClass(icon , $icon2);

			ace.settings.set('sidebar', 'collapsed');
		} else {
			ace.removeClass(sidebar , 'menu-min');
			ace.removeClass(icon , $icon2);
			ace.addClass(icon , $icon1);

			ace.settings.unset('sidebar', 'collapsed');
		}
	}
