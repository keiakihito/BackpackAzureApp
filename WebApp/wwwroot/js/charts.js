// Save this file as wwwroot/js/charts.js

// Initialize the chart library
window.initChartLibrary = function () {
    // This would normally initialize your chart library
    console.log("Chart library initialized");
};

// Render a chart based on data
window.renderChart = function (data) {
    // Example of rendering a chart - you'll replace this with actual chart library code
    // such as Chart.js, ApexCharts, or D3.js

    // For demonstration, we'll just create a simple placeholder
    const chartContainer = document.getElementById('chart-container');
    if (!chartContainer) return;

    // Sample data if none provided
    const sampleData = data || [
        { label: 'Category A', value: 25 },
        { label: 'Category B', value: 40 },
        { label: 'Category C', value: 15 },
        { label: 'Category D', value: 20 }
    ];

    // Clear any existing content
    chartContainer.innerHTML = '';

    // Create a simple bar chart visualization
    const chartWrapper = document.createElement('div');
    chartWrapper.style.height = '300px';
    chartWrapper.style.display = 'flex';
    chartWrapper.style.alignItems = 'flex-end';
    chartWrapper.style.gap = '12px';
    chartWrapper.style.padding = '10px';
    chartWrapper.style.maxWidth = '100%';
    chartWrapper.style.overflowX = 'auto';


    // Create bars
    sampleData.forEach(item => {
        const bar = document.createElement('div');
        bar.style.flex = '1';
        bar.style.backgroundColor = getRandomColor();
        bar.style.height = `${item.value * 2}px`;
        bar.style.display = 'flex';
        bar.style.flexDirection = 'column';
        bar.style.justifyContent = 'space-between';
        bar.style.alignItems = 'center';
        bar.style.padding = '8px 0';
        bar.style.borderRadius = '4px 4px 0 0';

        const value = document.createElement('span');
        value.textContent = item.value;
        value.style.color = 'white';
        value.style.fontWeight = 'bold';

        const barContainer = document.createElement('div');
        barContainer.style.display = 'flex';
        barContainer.style.flexDirection = 'column';
        barContainer.style.alignItems = 'center';

        bar.appendChild(value);
        barContainer.appendChild(bar);

        const label = document.createElement('div');
        label.textContent = item.label;
        label.style.marginTop = '8px';
        label.style.fontSize = '12px';
        label.style.textAlign = 'center';
        barContainer.appendChild(label);

        chartWrapper.appendChild(barContainer);
    });

    chartContainer.appendChild(chartWrapper);
};

// Helper function to generate random colors
function getRandomColor() {
    const colors = [
        '#4285F4', // Blue
        '#34A853', // Green
        '#FBBC05', // Yellow
        '#EA4335', // Red
        '#8E24AA', // Purple
        '#00ACC1'  // Teal
    ];
    return colors[Math.floor(Math.random() * colors.length)];
}