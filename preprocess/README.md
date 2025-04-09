# Dataset Overview

This project uses the following two CSV files provided by Kaggle:

- [`train.csv`](https://www.kaggle.com/competitions/playground-series-s5e2/data)
- [`training_extra.csv`](https://www.kaggle.com/competitions/playground-series-s5e2/data)

Please download both files and place them in the `data/` directory of this project.

Once the files are in place, you can generate a processed dataset used for AI analysis and querying by running the following command:

```bash
python combine_data.py
```

This will generate a cleaned and merged dataset as processed_data.csv, based on the fields such as brand, price, waterproof, weight, and size. This data is used as the source for downstream AI analysis and visualization.

## 📊 Profile Report Preview

If you'd like to view the generated data profile (processed_profile.html) directly in your browser without downloading the file, use the following preview link:

🔗 View processed_profile.html in your browser

This preview is powered by htmlpreview.github.io, a free static HTML viewer for GitHub repositories.
